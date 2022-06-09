using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Service.Services
{
    public class UserService : IUserService
    {
        private IRepository<UserEntity> _repository;
        private IUserDomain _userDomain;

        public UserService(IRepository<UserEntity> repository, IUserDomain userDomain)
        {
            _repository = repository;
            _userDomain = userDomain;
        }
        public async Task<UserEntity> Authenticate(string username, string password)
        {
            try
            {
                var userDb = (await _repository.Filter(c => c.Username == username)).FirstOrDefault();

                if (userDb != null && await _userDomain.IsValidPassword(password, userDb.Password))
                    userDb.Token = await _userDomain.GenerateJWTToken(userDb);
                else
                {
                    userDb = new UserEntity();
                    userDb.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.UserNotIdentified,
                        Type = ErrorCode.UserNotIdentified.ToString(),
                        Message = ErrorCode.UserNotIdentified.GetDescription()
                    };
                }

                return userDb;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<UserEntity> Delete(int id)
        {
            var userEntity = new UserEntity();

            try
            {
                var result = await _repository.Delete(id);
                if (result)
                    return userEntity;
                else
                {
                    userEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.UserNotIdentified,
                        Type = ErrorCode.UserNotIdentified.ToString(),
                        Message = ErrorCode.UserNotIdentified.GetDescription()
                    };
                    return userEntity;
                }
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<UserEntity>> GetAll()
        {
            try
            {
                return await _repository.Select();
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<UserEntity> GetById(int id)
        {
            try
            {
                return await _repository.SelectById(id);
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<UserEntity> Post(UserEntity request)
        {
            try
            {
                request.Password = await _userDomain.Encrypt(request.Password);

                var result = await _repository.Insert(request);
                if (result)
                {
                    request.Token = await _userDomain.GenerateJWTToken(request);
                    return request;
                }
                else
                {
                    request.Error = new ErrorDetail()
                    {
                        Code = 1,
                        Type = "",
                        Message = ""
                    };
                    return request;
                }
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<UserEntity> Put(UserEntity request)
        {
            var userEntity = new UserEntity();

            try
            {
                var user = await _repository.Filter(x => x.Username == request.Username);

                if (user.Any())
                {
                    request.Password = await _userDomain.Encrypt(request.Password);
                    request.CreatedAt = DateTime.Now;
                    request.Active = user.FirstOrDefault().Active;
                    request.Block = user.FirstOrDefault().Block;
                    request.Id = user.FirstOrDefault().Id;
                    request.RoleId = user.FirstOrDefault().RoleId;
                    request.Username = user.FirstOrDefault().Username;

                    var result = await _repository.Edit(request);
                    if (result)
                        return userEntity;
                    else
                    {
                        userEntity.Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.UserNotIdentified,
                            Type = ErrorCode.UserNotIdentified.ToString(),
                            Message = ErrorCode.UserNotIdentified.GetDescription()
                        };
                        return userEntity;
                    }
                }
                else
                {
                    userEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.UserNotIdentified,
                        Type = ErrorCode.UserNotIdentified.ToString(),
                        Message = ErrorCode.UserNotIdentified.GetDescription()
                    };
                    return userEntity;
                }
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public Task<int> ValidateJWTToken(string token)
        {
            return _userDomain.ValidateJWTToken(token);
        }
    }
}
