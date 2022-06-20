using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Presentation.Model.Request;
using TE.BE.City.Presentation.Model.Response;

namespace TE.BE.City.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class WaterController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IWaterService _waterService;

        /// <summary>
        /// Dependency injection to access Service layer.
        /// </summary>
        /// <param name="Service"></param>
        public WaterController(IWaterService Service, IMapper mapper) : base()
        {
            _mapper = mapper;
            _waterService = Service;
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
            waterEntity.HasWell = request.hasWell;
            waterEntity.HomeWithWater = request.homeWithWater;
            waterEntity.WaterMissedInAWeek = request.waterMissedInAWeek;
            waterEntity.CreatedAt = DateTime.Now.ToUniversalTime();
                
            var result = await _waterService.Post(waterEntity);
             
            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// Get all items based on filter.
        /// </summary>
        /// <param name="id"></param>
        [HttpPatch]
        public async Task<WaterSearchResponse> Patch([FromBody] WaterSearchRequest request)
        {
            var waterSearchResponseModel = new WaterSearchResponse();
            var waterEntity = new WaterEntity();
            waterEntity.Id = request.Id;

            if (waterEntity.Id > 0)
            {
                var userEntity = await _waterService.GetById(waterEntity.Id);
                var waterResponseModel = new WaterResponse();
                _mapper.Map(userEntity, waterResponseModel);
                waterSearchResponseModel.WaterList.Add(waterResponseModel);
            }
            else
            {
                waterEntity.StatusId = request.StatusId;
                waterEntity.CreatedAt = request.StartDate;
                waterEntity.EndDate = request.EndDate;

                var usersEntity = await _waterService.GetAll(request.Skip, request.Limit);
                _mapper.Map(usersEntity, waterSearchResponseModel.WaterList);
            }

            int limit = request.Limit == 0 ? WaterSearchRequest.LIMIT : request.Limit;
            waterSearchResponseModel.Page = request.Skip / limit;
            waterSearchResponseModel.Total = await _waterService.GetCount(waterEntity);

            return waterSearchResponseModel;
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
            waterEntity.Latitude = request.Latitude;
            waterEntity.Longitude = request.Longitude;
            waterEntity.HasWell = request.hasWell;
            waterEntity.HomeWithWater = request.homeWithWater;
            waterEntity.WaterMissedInAWeek = request.waterMissedInAWeek;
            waterEntity.UserId = request.UserId;
            waterEntity.StatusId = request.StatusId;
            waterEntity.CreatedAt = DateTime.Now.ToUniversalTime();

            return Response(true, await _waterService.Put(waterEntity));
        }

        /// <summary>
        /// delete one item.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return Response(true, await _waterService.Delete(id));    
        }
    }
}
