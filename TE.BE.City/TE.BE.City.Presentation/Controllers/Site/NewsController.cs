using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting.Enum;
using TE.BE.City.Presentation.Model.ViewModel;
using TE.BE.City.Domain.Caching;
using System.Runtime.Caching;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace TE.BE.City.Presentation.Controllers.Site
{
    [Route("site/[controller]")]
    public class NewsController : Controller
    {
        private const string cacheKey = "lupanh";
        private static IMicroCache<NewsViewModel> _stringCache;
        private readonly INewsService _newsService;
        private readonly IAsphaltService _asphaltService;
        private readonly ICollectService _collectService;
        private readonly ILightService _lightService;
        private readonly ISewerService _sewerService;
        private readonly ITrashService _trashService;
        private readonly IWaterService _waterService;
        private readonly IConfiguration _config;
        private GenerativeTool generativeTool;

        public NewsController(IConfiguration config,
            INewsService newsService, 
            IAsphaltService asphaltService,
            ICollectService collectService,
            ILightService lightService,
            ISewerService sewerService,
            ITrashService trashService,
            IWaterService waterService 
            )
        {
            _newsService = newsService;
            _asphaltService = asphaltService;
            _collectService = collectService;
            _lightService = lightService;
            _sewerService = sewerService;
            _trashService = trashService;
            _waterService = waterService;
            _config = config;
            _stringCache = new MicroCache<NewsViewModel>(System.Runtime.Caching.MemoryCache.Default);
            if (Enum.TryParse(typeof(GenerativeTool), _config["generativeTool"], true, out Object option))
                generativeTool = (GenerativeTool)option;
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<ActionResult> Index()
        //{
        //    var response = new NewsViewModel();

        //    response =  _stringCache.GetOrAdd(
        //        cacheKey,
        //        () => LoadData(),
        //        () => LoadCacheItemPolicy());

        //    return View(model: response);
        //}

        public NewsViewModel InternalIndex()
        {
            var response = new NewsViewModel();

            response = _stringCache.GetOrAdd(
                cacheKey,
                () => LoadData(),
                () => LoadCacheItemPolicy());

            return response;
        }

        private CacheItemPolicy LoadCacheItemPolicy()
        {
            var policy = new CacheItemPolicy();

            policy.Priority = CacheItemPriority.NotRemovable;

            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(int.Parse(_config["cacheNews"]));

            return policy;
        }

        private NewsViewModel LoadData() 
        {
            var response = Task.Run(() => LoadDataAsync());
            response.Wait();
            return response.Result;
        }

        private async Task<NewsViewModel> LoadDataAsync()
        {
            var response = new NewsViewModel();

            var newsEntity = new NewsEntity
            {
                NewsPriority = await _newsService.Next(),
                NewsTextEntity = await _newsService.Text(generativeTool)
            };

            newsEntity.NewsPriority.Address = newsEntity.NewsPriority.OccurrenceType switch
            {
                TypeIssue.Asphalt => await _asphaltService.GetLocationAddress(newsEntity.NewsPriority.OccurrenceId),
                TypeIssue.Collect => await _collectService.GetLocationAddress(newsEntity.NewsPriority.OccurrenceId),
                TypeIssue.Light => await _lightService.GetLocationAddress(newsEntity.NewsPriority.OccurrenceId),
                TypeIssue.Sewer => await _sewerService.GetLocationAddress(newsEntity.NewsPriority.OccurrenceId),
                TypeIssue.Trash => await _trashService.GetLocationAddress(newsEntity.NewsPriority.OccurrenceId),
                TypeIssue.Water => await _waterService.GetLocationAddress(newsEntity.NewsPriority.OccurrenceId),
                _ => newsEntity.NewsPriority.Address
            };

            switch (generativeTool)
            {
                case GenerativeTool.Local:
                    response.News = $"{newsEntity.NewsTextEntity.T1}" + $" {newsEntity.NewsPriority.OccurrenceType.GetDescription()}";
                    if (newsEntity.NewsPriority.Address != string.Empty)
                        response.News += $" no endereço {newsEntity.NewsPriority.Address}.";
                    else
                        response.News += $" no bairro Novo Horizonte.";
                    response.News += $" {newsEntity.NewsTextEntity.T2}";
                    break;
                case GenerativeTool.chatGPT:
                        var subject = $"{newsEntity.NewsTextEntity.T1}" + $" {newsEntity.NewsPriority.OccurrenceType} public service";
                        if(newsEntity.NewsPriority.Address != string.Empty)
                            subject += $" at {newsEntity.NewsPriority.Address}.";
                        else
                            subject += $" in Novo Horizonte neighbourhood.";
                        subject = $" {newsEntity.NewsTextEntity.T2}" + $" {newsEntity.NewsTextEntity.T3}";
                        response.News = await _newsService.GenerateNewsRecomendation(subject);
                        break;
                default:
                    response.News = "Nenhuma recomendação no momento.";
                    break;
            }
            return response;
        }
    }
}
