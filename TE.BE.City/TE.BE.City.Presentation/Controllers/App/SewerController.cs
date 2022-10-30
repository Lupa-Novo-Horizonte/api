using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting.Enum;
using TE.BE.City.Presentation.Model.Request;
using TE.BE.City.Presentation.Model.Response;

namespace TE.BE.City.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SewerController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ISewerService _sewerService;

        public SewerController(ISewerService sewerService, IMapper mapper)
        {
            _sewerService = sewerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Post new user.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SewerRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "userId").Value);

            var sewerEntity = new SewerEntity();
            sewerEntity.Longitude = request.Longitude;
            sewerEntity.Latitude = request.Latitude;
            sewerEntity.HasHomeSewer = request.HasHomeSewer;
            sewerEntity.HasHomeCesspool = request.HasHomeCesspool;
            sewerEntity.HasSanitationProject = request.HasSanitationProject;
            sewerEntity.CreatedAt = DateTime.Now;
            sewerEntity.UserId = userId;
            sewerEntity.StatusId = 1; // request.StatusId;

            var result = await _sewerService.Post(sewerEntity);

            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// Get all item if id is null or 0. Get one item by id.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        public async Task<SewerSearchResponse> Get(bool? closed, int skip=0, int limit=10, int id = 0)
        {
            var sewerSearchResponse = new SewerSearchResponse();
            
            if (id > 0)
            {
                var userEntity = await _sewerService.GetById(id);
                _mapper.Map(userEntity, sewerSearchResponse.Contacts);
            }
            else if (closed != null)
            {
                var userEntity = await _sewerService.GetClosed((bool)closed, skip, limit);
                _mapper.Map(userEntity, sewerSearchResponse.Contacts);
            }
            else
            {
                var usersEntity = await _sewerService.GetAll(skip, limit);
                _mapper.Map(usersEntity, sewerSearchResponse.Contacts);
            }

            sewerSearchResponse.Page = skip / limit;
            sewerSearchResponse.Total = await _sewerService.GetCount(closed);

            return sewerSearchResponse;
        }

        /// <summary>
        /// Update one item.
        /// </summary>
        /// <param name="request"></param>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] SewerRequest request)
        {
            var sewerEntity = new SewerEntity();
            sewerEntity.Id = request.Id;
            sewerEntity.Longitude = request.Longitude;
            sewerEntity.Latitude = request.Latitude;
            sewerEntity.HasHomeSewer = request.HasHomeSewer;
            sewerEntity.HasHomeCesspool = request.HasHomeCesspool;
            sewerEntity.HasSanitationProject = request.HasSanitationProject;
            sewerEntity.CreatedAt = DateTime.Now;
            sewerEntity.UserId = request.UserId;
            sewerEntity.StatusId = request.StatusId;

            var result = await _sewerService.Put(sewerEntity);

            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// delete one item.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _sewerService.Delete(id);

            return Response(result.IsSuccess,null);
        }
    }
}