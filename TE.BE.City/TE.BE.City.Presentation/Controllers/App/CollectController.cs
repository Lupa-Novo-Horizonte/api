using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    public class CollectController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ICollectService _collectService;

        public CollectController(ICollectService collectService, IMapper mapper)
        {
            _collectService = collectService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all item.
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        [HttpGet]
        public async Task<CollectSearchResponse> Get(int id, int skip = 0, int limit = 50)
        {
            var collectSearchResponse = new CollectSearchResponse();
            IEnumerable<CollectEntity> collectEntity;

            if (id > 0)
            {
                collectEntity = await _collectService.GetById(id);
                collectSearchResponse.Total = collectEntity.Count();
            }
            else
            {
                collectEntity = await _collectService.GetAll(skip, limit);
                collectSearchResponse.Total = await _collectService.GetCount(false);
            }

            _mapper.Map(collectEntity, collectSearchResponse.CollectList);

            return collectSearchResponse;
        }

        /// <summary>
        /// Post new user.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CollectRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "userId").Value);

            var collectEntity = new CollectEntity();
            collectEntity.Longitude = request.Longitude;
            collectEntity.Latitude = request.Latitude;
            collectEntity.HasCollect = request.HasCollect;
            collectEntity.HowManyTimes = request.HowManyTimes;
            collectEntity.HasSelectiveCollect = request.HasSelectiveCollect;
            collectEntity.CreatedAt = DateTime.Now.ToUniversalTime();
            collectEntity.UserId = userId;
            collectEntity.StatusId = 1; // request.StatusId;

            var result = await _collectService.Post(collectEntity);

            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// Update one item.
        /// </summary>
        /// <param name="request"></param>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] CollectRequest request)
        {
            var collectEntity = new CollectEntity();
            collectEntity.Id = request.Id;
            collectEntity.Longitude = request.Longitude;
            collectEntity.Latitude = request.Latitude;
            collectEntity.HasCollect = request.HasCollect;
            collectEntity.HowManyTimes = request.HowManyTimes;
            collectEntity.HasSelectiveCollect = request.HasSelectiveCollect;
            collectEntity.CreatedAt = DateTime.Now.ToUniversalTime();
            collectEntity.UserId = request.UserId;
            collectEntity.StatusId = request.StatusId;

            var result = await _collectService.Put(collectEntity);

            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// delete one item.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _collectService.Delete(id);

            return Response(result.IsSuccess, null);
        }
    }
}
