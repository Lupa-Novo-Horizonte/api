using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using System.Linq;
using TE.BE.City.Infra.CrossCutting.Enum;
using LinqKit;

namespace TE.BE.City.Service.Services
{
    public class LightService : ILightService
    {
        private readonly IRepository<LightEntity> _repository;

        public LightService(IRepository<LightEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LightEntity>> GetAll(int skip, int limit)
        {
            var lightsEntity = new List<LightEntity>();

            try
            {
                IEnumerable<LightEntity> result;

                if (skip == 0 && limit == 0)
                    result = await _repository.Select();
                else
                    result = await _repository.SelectWithPagination(skip, limit);

                if (result != null)
                    return result;
                else
                {
                    var lightEntity = new LightEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    lightsEntity.Add(lightEntity);
                }

                return lightsEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<LightEntity> Get()
        {
            try
            {
                var result = await _repository.Select();
                return result.FirstOrDefault();
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<LightEntity> Delete(int id)
        {
            var lightEntity = new LightEntity();

            try
            {
                var result = await _repository.Delete(id);
                if (result)
                    return lightEntity;
                else
                {
                    lightEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.SearchHasNoResult,
                        Type = ErrorCode.SearchHasNoResult.ToString(),
                        Message = ErrorCode.SearchHasNoResult.GetDescription()
                    };
                }

                return lightEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<LightEntity> Post(LightEntity request)
        {
            var lightEntity = new LightEntity();

            try
            {
                var result = await _repository.Insert(request);

                if (result)
                    return lightEntity;
                else
                {
                    lightEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return lightEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<LightEntity> Put(LightEntity request)
        {
            var lightEntity = new LightEntity();

            try
            {
                lightEntity = await _repository.SelectById(request.Id);
                lightEntity.CreatedAt = request.CreatedAt;
                lightEntity.Longitude = request.Longitude;
                lightEntity.Latitude = request.Latitude;
                lightEntity.HasLight = request.HasLight;
                lightEntity.IsItWorking = request.IsItWorking;
                lightEntity.HasLosesCable = request.HasLosesCable;
                lightEntity.Path = request.Path;
                lightEntity.UserId = request.UserId;
                lightEntity.StatusId = request.StatusId;

                var result = await _repository.Edit(lightEntity);

                if (result)
                    return lightEntity;
                else
                {
                    lightEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return lightEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<LightEntity>> GetFilter(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var predicate = PredicateBuilder.New<LightEntity>(true);

                if (startDate != null && startDate > DateTime.MinValue)
                    predicate.And(model => model.CreatedAt.Date >= startDate);
                if (endDate != null && endDate > DateTime.MinValue)
                    predicate.And(model => model.CreatedAt.Date <= endDate);

                return await _repository.Filter(predicate);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
