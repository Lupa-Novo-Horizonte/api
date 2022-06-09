using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Service.Services
{
    public class SewerService : ISewerService
    {
        private readonly IRepository<SewerEntity> _repository;
        
        public SewerService(IRepository<SewerEntity> repository)
        {
            _repository = repository;
        }
        
        public async Task<SewerEntity> Delete(int id)
        {
            var contactEntity = new SewerEntity();

            try
            {
                var result = await _repository.Delete(id);
                if (result)
                    return contactEntity;
                else
                {
                    contactEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.SearchHasNoResult,
                        Type = ErrorCode.SearchHasNoResult.ToString(),
                        Message = ErrorCode.SearchHasNoResult.GetDescription()
                    };
                }

                return contactEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<SewerEntity>> GetAll(int skip, int limit)
        {
            var contactsEntity = new List<SewerEntity>();

            try
            {
                IEnumerable<SewerEntity> result;

                if (skip == 0 && limit == 0)
                    result = await _repository.Select();
                else
                    result = await _repository.SelectWithPagination(skip, limit);

                if (result != null)
                    return result;
                else
                {
                    var contactEntity = new SewerEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    contactsEntity.Add(contactEntity);
                }

                return contactsEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<SewerEntity>> GetById(int id)
        {
            var contactsEntity = new List<SewerEntity>();

            try
            {
                var result =  await _repository.SelectById(id);

                if(result != null)
                    contactsEntity.Add(result);
                else
                {
                    var contactEntity = new SewerEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    contactsEntity.Add(contactEntity);
                }

                return contactsEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<SewerEntity>> GetClosed(bool closed, int skip, int limit)
        {
            var contactsEntity = new List<SewerEntity>();

            try
            {
                IEnumerable<SewerEntity> result;

                if (skip == 0 && limit ==0)
                    result = await _repository.Filter(c => c.EndDate <= DateTime.Today);
                else
                    result = await _repository.FilterWithPagination(c => c.EndDate <= DateTime.Today, skip, limit);

                if (result != null)
                    return result;
                else
                {
                    var contactEntity = new SewerEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    contactsEntity.Add(contactEntity);
                }

                return contactsEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<int> GetCount(bool? closed)
        {
            try
            {
                if (closed != null)
                {
                    var result = await _repository.Filter(c => c.EndDate <= DateTime.Today);
                    return result.Count();
                }
                else
                    return await _repository.SelectCount();
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<SewerEntity> Post(SewerEntity request)
        {
            var contactEntity = new SewerEntity();

            try
            {
                var result = await _repository.Insert(request);

                if (result)
                    return contactEntity;
                else
                {
                    contactEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return contactEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<SewerEntity> Put(SewerEntity request)
        {
            var sewerEntity = new SewerEntity();

            try
            {
                sewerEntity = await _repository.SelectById(request.Id);

                if (sewerEntity != null)
                {
                    sewerEntity.Longitude = request.Longitude;
                    sewerEntity.Latitude = request.Latitude;
                    sewerEntity.HasHomeSewer = request.HasHomeSewer;
                    sewerEntity.HasHomeCesspool = request.HasHomeCesspool;
                    sewerEntity.DoesCityHallCleanTheSewer = request.DoesCityHallCleanTheSewer;
                    sewerEntity.CreatedAt = DateTime.Now.ToUniversalTime();
                    sewerEntity.UserId = request.UserId;
                    sewerEntity.StatusId = request.StatusId;

                    var result = await _repository.Edit(sewerEntity);

                    if (result)
                        return sewerEntity;
                    else
                    {
                        sewerEntity.Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.InsertContactFail,
                            Type = ErrorCode.InsertContactFail.ToString(),
                            Message = ErrorCode.InsertContactFail.GetDescription()
                        };
                    }
                }
                else
                {
                    sewerEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return sewerEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }
    }
}
