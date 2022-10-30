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
    public class PublicServiceController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IPublicServiceService _publicServiceService;

        public PublicServiceController(IPublicServiceService publicServiceService, IMapper mapper)
        {
            _publicServiceService = publicServiceService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ocorrencyId"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        [HttpGet]
        public async Task<PublicServiceSearchResponse> Get(int id, int ocorrencyId, int skip = 0, int limit = 50)
        {
            var publicServiceSearchResponse = new PublicServiceSearchResponse();

            if (id > 0)
            {
                var publicServiceEntity = await _publicServiceService.GetById(id);
                _mapper.Map(publicServiceEntity, publicServiceSearchResponse.PublicServiceList);
                publicServiceSearchResponse.Total = publicServiceEntity.Count();
            }
            else if (ocorrencyId > 0)
            {
                var publicServiceEntity = await _publicServiceService.GetByOcorrencyId(false, ocorrencyId);
                _mapper.Map(publicServiceEntity, publicServiceSearchResponse.PublicServiceList);
                publicServiceSearchResponse.Total = publicServiceEntity.Count();
            }
            else
            {
                var publicServiceEntity = await _publicServiceService.GetAll(skip, limit);
                _mapper.Map(publicServiceEntity, publicServiceSearchResponse.PublicServiceList);
                publicServiceSearchResponse.Total = await _publicServiceService.GetCount(false);
            }

            return publicServiceSearchResponse;
        }

        /// <summary>
        /// Post new item.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PublicServiceRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "userId").Value);

            var publicServiceEntity = new PublicServiceEntity();
            publicServiceEntity.Longitude = request.Longitude;
            publicServiceEntity.Latitude = request.Latitude;
            publicServiceEntity.Service = request.Service;
            publicServiceEntity.CreatedAt = DateTime.Now;
            publicServiceEntity.UserId = userId; // request.UserId;
            publicServiceEntity.StatusId = 1; // request.StatusId;

            var result = await _publicServiceService.Post(publicServiceEntity);

            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// Update one item.
        /// </summary>
        /// <param name="request"></param>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PublicServiceRequest request)
        {
            var publicServiceEntity = new PublicServiceEntity();
            publicServiceEntity.Id = request.Id;
            publicServiceEntity.Latitude = request.Latitude;
            publicServiceEntity.Longitude = request.Longitude;
            publicServiceEntity.Service = request.Service;
            publicServiceEntity.CreatedAt = DateTime.Now;

            var result = await _publicServiceService.Put(publicServiceEntity);

            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// delete one item.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _publicServiceService.Delete(id);

            return Response(result.IsSuccess, null);
        }
    }
}
