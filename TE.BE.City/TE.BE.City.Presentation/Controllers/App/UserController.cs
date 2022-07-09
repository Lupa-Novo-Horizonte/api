using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Presentation.Model.Request;
using TE.BE.City.Presentation.Model.Response;

namespace TE.BE.City.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<AuthenticateResponse> Authenticate([FromBody] AuthenticateRequest model)
        {
            var authenticateResponse = new AuthenticateResponse();
            
            var userEntity = await _userService.Authenticate(model.Username, model.Password);

            _mapper.Map(userEntity, authenticateResponse);
            
            return authenticateResponse;
        }

        /// <summary>
        /// If the requested to this method worked is because dotNet validate the token sent in the authorize.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("validateToken")]
        public async Task<bool> ValidateToken()
        {
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Request recover password. E-mail sent to user.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("recovery")]
        [AllowAnonymous]
        public async Task<UserResponse> Recovery([FromBody] UserRequest request)
        {
            var userResponse = new UserResponse();

            var usersEntity = await _userService.Recovery(request.Username);

            _mapper.Map(usersEntity, userResponse);

            return userResponse;
        }
        
        /// <summary>
        /// Post new user.
        /// </summary>
        /// <param name="request"></param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<UserResponse> Post([FromBody] UserRequest request)
        {
            var userResponse = new UserResponse();

            var userEntity = new UserEntity();
            userEntity.FirstName = request.FirstName;
            userEntity.LastName = request.LastName;
            userEntity.Username = request.Username;
            userEntity.Password = request.Password;
            userEntity.Active = true;
            userEntity.Block = false;
            userEntity.RoleId = request.RoleId != 0 ? request.RoleId : 1;
            userEntity.CreatedAt = DateTime.Now.ToLocalTime();

            var usersEntity = await _userService.Post(userEntity);
            _mapper.Map(usersEntity, userResponse);

            return userResponse;
        }

        /// <summary>
        /// Get all item if id is null or 0. Get one item by id.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        public async Task<IEnumerable<UserResponse>> Get(int id = 0)
        {
            var usersResponse = new List<UserResponse>();
           
            if (id > 0)
            {
                var userEntity = await _userService.GetById(id);
                _mapper.Map(userEntity, usersResponse);
            }
            else
            {
                var usersEntity = await _userService.GetAll();
                _mapper.Map(usersEntity, usersResponse);
            }
            
            return usersResponse;
        }

        /// <summary>
        /// Update one item.
        /// </summary>
        /// <param name="request"></param>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UserRequest request)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.Id = request.Id;
            userEntity.FirstName = request.FirstName;
            userEntity.LastName = request.LastName;
            userEntity.Username = request.Username;
            userEntity.Password = request.Password;
            userEntity.RoleId = request.RoleId != 0 ? request.RoleId : 1;
            userEntity.CreatedAt = DateTime.Now.ToLocalTime();

            var result = await _userService.Put(userEntity);

            return Response(result.IsSuccess, null);
        }

        /// <summary>
        /// delete one item.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);
            return Response(result.IsSuccess, null);
        }
    }
}
