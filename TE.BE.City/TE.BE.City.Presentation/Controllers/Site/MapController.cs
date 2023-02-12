using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using TE.BE.City.Infra.CrossCutting.Enum;
using TE.BE.City.Presentation.Model.Response;

namespace TE.BE.City.Presentation.Controllers.Site
{
    [Authorize]
    [Route("api/site/[controller]")]
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
        public async Task<MapResponse> Get([FromQuery] string longStartDate, [FromQuery] string longEndDate, [FromQuery] TypeIssue typeIssue)
        {
            var mapResponse = new MapResponse();

            DateTime? startDate = long.Parse(longStartDate) > 0 ? new DateTime(long.Parse(longStartDate)) : DateTime.MinValue;
            DateTime? endDate = long.Parse(longEndDate) > 0 ? new DateTime(long.Parse(longEndDate)) : DateTime.MinValue;
            
            IEnumerable<WaterEntity> waterEntity = new List<WaterEntity>();
            IEnumerable<LightEntity> lightEntity = new List<LightEntity>();
            IEnumerable<TrashEntity> trashEntity = new List<TrashEntity>();
            IEnumerable<CollectEntity> collectEntity = new List<CollectEntity>();
            IEnumerable<SewerEntity> sewerEntity = new List<SewerEntity>();
            IEnumerable<AsphaltEntity> asphaltEntity = new List<AsphaltEntity>();
            IEnumerable<PublicServiceEntity> publicServiceEntity = new List<PublicServiceEntity>();

            if (typeIssue == TypeIssue.All || typeIssue == TypeIssue.Water)
                waterEntity = await _waterService.GetFilter(startDate, endDate);

            if (typeIssue == TypeIssue.All || typeIssue == TypeIssue.Light)
                lightEntity = await _lightService.GetFilter(startDate, endDate);

            if (typeIssue == TypeIssue.All || typeIssue == TypeIssue.Trash)
                trashEntity = await _trashService.GetFilter(startDate, endDate);

            if (typeIssue == TypeIssue.All || typeIssue == TypeIssue.Collect)
                collectEntity = await _collectService.GetFilter(startDate, endDate);

            if (typeIssue == TypeIssue.All || typeIssue == TypeIssue.Sewer)
                sewerEntity = await _sewerService.GetFilter(startDate, endDate);

            if (typeIssue == TypeIssue.All || typeIssue == TypeIssue.Asphalt)
                asphaltEntity = await _asphaltService.GetFilter(startDate, endDate);

            if (typeIssue == TypeIssue.All || typeIssue == TypeIssue.PublicService)
                publicServiceEntity = await _publicServiceService.GetFilter(startDate, endDate);
            

            foreach (var item in waterEntity.ToList())
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
