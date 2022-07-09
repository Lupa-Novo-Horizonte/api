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
    public class LightController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILightService _lightService;

        public LightController(ILightService lightService, IMapper mapper)
        {
            _lightService = lightService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all item if id is null or 0. Get one item by id.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        public async Task<LightResponse> Get()
        {
            var lightResponse = new LightResponse();

            var lightEntity = await _lightService.Get();
            _mapper.Map(lightEntity, lightResponse);

            return lightResponse;
        }

        /// <summary>
        /// Post new user.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LightRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "userId").Value);

            var lightEntity = new LightEntity();
            lightEntity.Longitude = request.Longitude;
            lightEntity.Latitude = request.Latitude;
            lightEntity.HasLight = request.HasLight;
            lightEntity.IsItWorking = request.IsItWorking;
            lightEntity.HasLosesCable = request.HasLosesCable;
            lightEntity.CreatedAt = DateTime.Now.ToLocalTime();
            lightEntity.UserId = userId;
            lightEntity.StatusId = 1; // request.StatusId;

            var result = await _lightService.Post(lightEntity);

            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// Update one item.
        /// </summary>
        /// <param name="request"></param>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] LightRequest request)
        {
            var lightEntity = new LightEntity();
            lightEntity.Id = request.Id;
            lightEntity.Longitude = request.Longitude;
            lightEntity.Latitude = request.Latitude;
            lightEntity.HasLight = request.HasLight;
            lightEntity.IsItWorking = request.IsItWorking;
            lightEntity.HasLosesCable = request.HasLosesCable;
            lightEntity.CreatedAt = DateTime.Now.ToLocalTime();
            lightEntity.UserId = request.UserId;
            lightEntity.StatusId = request.StatusId;

            var result = await _lightService.Put(lightEntity);

            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// delete one item.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _lightService.Delete(id);

            return Response(result.IsSuccess, null);
        }
    }
}
