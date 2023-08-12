using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Presentation.Model.Request;
using TE.BE.City.Presentation.Model.Response;
using TE.BE.City.Service.Services;

namespace TE.BE.City.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class WaterController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IWaterService _waterService;
        private readonly IBackgroundService _backgroundService;

        /// <summary>
        /// Dependency injection to access Service layer.
        /// </summary>
        /// <param name="Service"></param>
        public WaterController(IWaterService Service, IMapper mapper, IBackgroundService backgroundService) : base()
        {
            _mapper = mapper;
            _waterService = Service;
            _backgroundService = backgroundService;
        }

        /// <summary>
        /// Post new  location.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]WaterRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "userId").Value);

            var waterEntity = new WaterEntity();
            waterEntity.Latitude = request.Latitude;
            waterEntity.Longitude = request.Longitude;
            waterEntity.StatusId = 1; // request.StatusId;
            waterEntity.UserId = userId;
            waterEntity.HasWell = request.HasWell;
            waterEntity.HomeWithWater = request.HomeWithWater;
            waterEntity.WaterMissedInAWeek = request.WaterMissedInAWeek;
            waterEntity.HasSanitationProject = request.HasSanitationProject;
            waterEntity.CreatedAt = DateTime.Now;
                
            var result = await _waterService.Post(waterEntity);

            // Fire and forget
            _backgroundService.ExecuteAsync();

            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// Get item by id or send 0 for all.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        [HttpGet]
        public async Task<WaterSearchResponse> Get(int id, int skip = 0, int limit = 50)
        {
            var waterSearchResponse = new WaterSearchResponse();

            if (id > 0)
            {
                var waterEntity = await _waterService.GetById(id);
                _mapper.Map(waterEntity, waterSearchResponse.WaterList);
                waterSearchResponse.Total = waterSearchResponse.WaterList.Count();
            }
            else
            {
                var waterEntity = await _waterService.GetAll(skip, limit);
                _mapper.Map(waterEntity, waterSearchResponse.WaterList);
                waterSearchResponse.Total = waterEntity.Count();
            }

            return waterSearchResponse;
        }

        /// <summary>
        /// Update one item.
        /// </summary>
        /// <param name="request"></param>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] WaterRequest request)
        {
            WaterEntity waterEntity = new WaterEntity();
            waterEntity.Id = request.Id;
            waterEntity.StatusId = request.StatusId;
            
            var result = await _waterService.Put(waterEntity);

            return Response(result, null);
        }

        /// <summary>
        /// delete one item.
        /// </summary>
        /// <param name="id"></param>
        /*[HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _waterService.Delete(id);

            return Response(result, null);    
        }*/
    }
}
