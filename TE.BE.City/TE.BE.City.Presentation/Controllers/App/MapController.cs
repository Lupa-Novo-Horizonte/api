using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Presentation.Model.Response;
using TE.BE.City.Infra.CrossCutting;

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

        public MapController(IMapper mapper, 
            IAsphaltService asphaltService, 
            ICollectService collectService, 
            ILightService lightService, 
            ISewerService sewerService,
            ITrashService trashService,
            IWaterService waterService)
        {
            _mapper = mapper;
            _asphaltService = asphaltService;
            _collectService = collectService;
            _lightService = lightService;   
            _sewerService = sewerService;
            _trashService = trashService;
            _waterService = waterService;
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
            //_mapper.Map(watersEntity.ToList(), mapResponse.WaterList);

            var lightEntity = await _lightService.GetAll(0, 0);
            //_mapper.Map(lightEntity.ToList(), mapResponse.LightList);

            var trashEntity = await _trashService.GetAll(0,0);
            //_mapper.Map(trashEntity.ToList(), mapResponse.TrashList);

            var collectEntity = await _collectService.GetAll(0, 0);
            //_mapper.Map(collectEntity.ToList(), mapResponse.CollectList);

            var sewerEntity = await _sewerService.GetAll(0, 0);
            //_mapper.Map(sewerEntity.ToList(), mapResponse.SewerList);

            var asphaltEntity = await _asphaltService.GetAll(0, 0);
            //_mapper.Map(asphaltEntity.ToList(), mapResponse.AsphaltList);

            foreach (var item in watersEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Água Potável",
                    Description = $"Possui água? {item.HomeWithWater.ToSimNao()} | Dias faltam água na semana? {item.WaterMissedInAWeek} | Possui poço? {item.HasWell.ToSimNao()} | Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in lightEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Iluminação Pública",
                    Description = $"Possui poste? {item.HasLight.ToSimNao()} | Luzes funcionando? {item.IsItWorking.ToSimNao()} | Há fios soltos? {item.HasLosesCable.ToSimNao()} | Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in trashEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Limpeza Urbana",
                    Description = $"Existe limpeza da rua? {item.HasRoadcleanUp.ToSimNao()} | Qual a frequência? {item.HowManyTimes} | Existe lixo acumulado? {item.HasAccumulatedTrash.ToSimNao()} | Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in collectEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Coleta de Lixo",
                    Description = $"Existe coleta? {item.HasCollect.ToSimNao()} | Qual a frequência? {item.HowManyTimes} | Existe coleta seletiva? {item.HasSelectiveCollect.ToSimNao()} | Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in sewerEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Tratamento de Esgoto",
                    Description = $"Existe coleta de esgoto? {item.HasHomeSewer.ToSimNao()} | Possui fossa? {item.HasHomeCesspool.ToSimNao()} | Prefeitura limpa o esgoto? {item.DoesCityHallCleanTheSewer.ToSimNao()} | Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in asphaltEntity.ToList())
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Calçadas e Asfalto",
                    Description = $"A via é asfaltada? {item.IsPaved.ToSimNao()} | Possui buracos? {item.HasHoles.ToSimNao()} | Calçadas pavimentadas? {item.HasPavedSidewalks.ToSimNao()} | Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            return mapResponse;
        }
    }
}
