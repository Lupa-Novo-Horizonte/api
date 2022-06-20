using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using System.Linq;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Service.Services
{
    public class CollectService : ICollectService
    {
        private readonly IRepository<CollectEntity> _repository;

        public CollectService(IRepository<CollectEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CollectEntity>> GetAll(int skip, int limit)
        {
            var contactsEntity = new List<CollectEntity>();

            try
            {
                IEnumerable<CollectEntity> result;

                if (skip == 0 && limit == 0)
                    result = await _repository.Select();
                else
                    result = await _repository.SelectWithPagination(skip, limit);

                if (result != null)
                    return result;
                else
                {
                    var contactEntity = new CollectEntity()
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

        public async Task<IEnumerable<CollectEntity>> GetById(int id)
        {
            var contactsEntity = new List<CollectEntity>();

            try
            {
                var result = await _repository.SelectById(id);

                if (result != null)
                    contactsEntity.Add(result);
                else
                {
                    var contactEntity = new CollectEntity()
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

        public async Task<CollectEntity> Delete(int id)
        {
            var collectEntity = new CollectEntity();

            try
            {
                var result = await _repository.Delete(id);
                if (result)
                    return collectEntity;
                else
                {
                    collectEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.SearchHasNoResult,
                        Type = ErrorCode.SearchHasNoResult.ToString(),
                        Message = ErrorCode.SearchHasNoResult.GetDescription()
                    };
                }

                return collectEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<CollectEntity> Post(CollectEntity request)
        {
            var collectEntity = new CollectEntity();

            try
            {
                var result = await _repository.Insert(request);

                if (result)
                    return collectEntity;
                else
                {
                    collectEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return collectEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<CollectEntity> Put(CollectEntity request)
        {
            var collectEntity = new CollectEntity();

            try
            {
                collectEntity = await _repository.SelectById(request.Id);
                collectEntity.Id = request.Id;
                collectEntity.Longitude = request.Longitude;
                collectEntity.Latitude = request.Latitude;
                collectEntity.HasCollect = request.HasCollect;
                collectEntity.HowManyTimes = request.HowManyTimes;
                collectEntity.HasSelectiveCollect = request.HasSelectiveCollect;
                collectEntity.CreatedAt = DateTime.Now.ToUniversalTime();
                collectEntity.UserId = request.UserId;
                collectEntity.StatusId = request.StatusId;

                var result = await _repository.Edit(collectEntity);

                if (result)
                    return collectEntity;
                else
                {
                    collectEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return collectEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }
    }
}
