using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
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
                    Type = Infra.CrossCutting.Enum.TypeIssue.Water,
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
                    Type = Infra.CrossCutting.Enum.TypeIssue.Light,
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
                    Type = Infra.CrossCutting.Enum.TypeIssue.Trash,
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
                    Type = Infra.CrossCutting.Enum.TypeIssue.Collect,
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
                    Type = Infra.CrossCutting.Enum.TypeIssue.Sewer,
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
                    Type = Infra.CrossCutting.Enum.TypeIssue.Asphalt,
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
                    Type = Infra.CrossCutting.Enum.TypeIssue.PublicService,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Serviços e Referências",
                    Description = $"{item.Service} | Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            return mapResponse;
        }
    }
}
