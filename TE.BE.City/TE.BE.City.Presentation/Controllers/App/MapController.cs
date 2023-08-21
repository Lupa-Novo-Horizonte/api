using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cmp;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using TE.BE.City.Infra.CrossCutting.Enum;
using TE.BE.City.Presentation.Model.Response;

namespace TE.BE.City.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MapController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IAsphaltService _asphaltService;
        private readonly ICollectService _collectService;
        private readonly ILightService _lightService;
        private readonly ISewerService _sewerService;
        private readonly ITrashService _trashService;
        private readonly IWaterService _waterService;
        private readonly IPublicServiceService _publicServiceService;

        public MapController(IMapper mapper, 
            IAsphaltService asphaltService, 
            ICollectService collectService, 
            ILightService lightService, 
            ISewerService sewerService,
            ITrashService trashService,
            IWaterService waterService,
            IPublicServiceService publicServiceService)
        {
            _mapper = mapper;
            _asphaltService = asphaltService;
            _collectService = collectService;
            _lightService = lightService;   
            _sewerService = sewerService;
            _trashService = trashService;
            _waterService = waterService;
            _publicServiceService = publicServiceService;
        }

        /// <summary>
        /// Return list of all entries for all issues
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MapResponse> Get()
        {
            var mapResponse = new MapResponse();

            var watersEntity = await _waterService.GetAll(0, 0);
            var lightEntity = await _lightService.GetAll(0, 0);
            var trashEntity = await _trashService.GetAll(0,0);
            var collectEntity = await _collectService.GetAll(0, 0);
            var sewerEntity = await _sewerService.GetAll(0, 0);
            var asphaltEntity = await _asphaltService.GetAll(0, 0);
            var publicServiceEntity = await _publicServiceService.GetAll(0, 0);

            foreach (var item in watersEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Id = item.Id,
                    Type = Infra.CrossCutting.Enum.TypeIssue.Water,
                    IsProblem = item.IsProblem,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Água Potável",
                    Description = $"- Possui poço amazônico? {item.HasWell.ToSimNao()} |- Há água encanada? {item.HomeWithWater.ToSimNao()} |- Quantos dias faltam água na semana? {item.WaterMissedInAWeek} |- Alguma obra de saneamento está sendo executada? {item.HasWell.ToSimNao()} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in lightEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Id = item.Id,
                    Type = Infra.CrossCutting.Enum.TypeIssue.Light,
                    IsProblem = item.IsProblem,
                    Latitude = string.IsNullOrEmpty(item.Latitude) ? null : float.Parse(item.Latitude),
                    Longitude = string.IsNullOrEmpty(item.Longitude) ? null : float.Parse(item.Longitude),
                    Path = item.Path,
                    Title = "Iluminação Pública",
                    Description = $"- Possui poste? {item.HasLight.ToSimNao()} |- As luzes estão funcionando? {item.IsItWorking.ToSimNao()} |- Há fios elétricos soltos? {item.HasLosesCable.ToSimNao()} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in trashEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Id = item.Id,
                    Type = Infra.CrossCutting.Enum.TypeIssue.Trash,
                    IsProblem = item.IsProblem,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Limpeza Urbana",
                    Description = $"- A prefeitura faz a limpeza? {item.HasRoadCleanUp.ToSimNao()} |- Se sim, qual a frequência semanal? {item.HowManyTimes} |- Existe lixo acumulado? {item.HasAccumulatedTrash.ToSimNao()} |- A prefeitura faz a capinagem? {item.HasAccumulatedTrash.ToSimNao()} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in collectEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Id = item.Id,
                    Type = Infra.CrossCutting.Enum.TypeIssue.Collect,
                    IsProblem = item.IsProblem,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Coleta de Lixo",
                    Description = $"- Há coleta de lixo? {item.HasCollect.ToSimNao()} |- Qual a frequência semanal? {item.HowManyTimes} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in sewerEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Id = item.Id,
                    Type = Infra.CrossCutting.Enum.TypeIssue.Sewer,
                    IsProblem = item.IsProblem,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Tratamento de Esgoto",
                    Description = $"- Há coleta ou tratamento de esgoto? {item.HasHomeSewer.ToSimNao()} |- Possui fossa? {item.HasHomeCesspool.ToSimNao()} |- Alguma obra de saneamento está sendo executada? {item.HasSanitationProject.ToSimNao()} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in asphaltEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Id = item.Id,
                    Type = Infra.CrossCutting.Enum.TypeIssue.Asphalt,
                    IsProblem = item.IsProblem,
                    Latitude = string.IsNullOrEmpty(item.Latitude) ? null : float.Parse(item.Latitude),
                    Longitude = string.IsNullOrEmpty(item.Longitude) ? null : float.Parse(item.Longitude),
                    Path = item.Path,
                    Title = "Calçadas e Asfalto",
                    Description = $"- A via é asfaltada? {item.IsPaved.ToSimNao()} |- A via possui buracos ou crateras? {item.HasHoles.ToSimNao()} |- Há calçadas pavimentadas de acordo com os requisitos municipais? {item.HasPavedSidewalks.ToSimNao()} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in publicServiceEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Id = item.Id,
                    Type = Infra.CrossCutting.Enum.TypeIssue.PublicService,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Serviços e Referências",
                    Description = $"{item.Service} | Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            return mapResponse;
        }

        [HttpGet]
        [Route("report")]
        public async Task<ChartResponse> Report(int userId)
        {
            var chartResponse = new ChartResponse();
            chartResponse.ChartTable = new System.Collections.Generic.Dictionary<TypeIssue, ChartTable>();

            var waterEntityList = await _waterService.GetAllByUser(userId);
            var lightEntityList = await _lightService.GetAllByUser(userId);
            var trashEntityList = await _trashService.GetAllByUser(userId);
            var collectEntityList = await _collectService.GetAllByUser(userId);
            var sewerEntityList = await _sewerService.GetAllByUser(userId);
            var asphaltEntityList = await _asphaltService.GetAllByUser(userId);

            int countProblemAsphalt = asphaltEntityList.Count(c => c.IsProblem);
            int countNoProblemAslphat = asphaltEntityList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Asphalt, new ChartTable()
            {
                ProblemCount = countProblemAsphalt,
                NoProblemCount = countNoProblemAslphat
            });

            int countProblemCollect = collectEntityList.Count(c => c.IsProblem);
            int countNoProblemCollect = collectEntityList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Collect, new ChartTable()
            {
                ProblemCount = countProblemCollect,
                NoProblemCount = countNoProblemCollect
            });

            int countProblemLight = lightEntityList.Count(c => c.IsProblem);
            int countNoProblemLight = lightEntityList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Light, new ChartTable()
            {
                ProblemCount = countProblemLight,
                NoProblemCount = countNoProblemLight
            });

            int countProblemSewer = sewerEntityList.Count(c => c.IsProblem);
            int countNoProblemSewer = sewerEntityList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Sewer, new ChartTable()
            {
                ProblemCount = countProblemSewer,
                NoProblemCount = countNoProblemSewer
            });

            int countProblemTrash = trashEntityList.Count(c => c.IsProblem);
            int countNoProblemTrash = trashEntityList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Trash, new ChartTable()
            {
                ProblemCount = countProblemTrash,
                NoProblemCount = countNoProblemTrash
            });

            int countProblemWater = waterEntityList.Count(c => c.IsProblem);
            int countNoProblemWater = waterEntityList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Water, new ChartTable()
            {
                ProblemCount = countProblemWater,
                NoProblemCount = countNoProblemWater
            });

            chartResponse.ChartTable.Add(TypeIssue.All, new ChartTable()
            {
                ProblemCount = countProblemAsphalt + countProblemCollect + countProblemLight + countProblemSewer + countProblemTrash + countProblemWater,
                NoProblemCount = countNoProblemAslphat + countNoProblemCollect + countNoProblemLight + countNoProblemSewer + countNoProblemTrash + countNoProblemWater
            });

            return chartResponse;
        }
    }
}
