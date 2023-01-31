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
    public class AsphaltService : IAsphaltService
    {
        private readonly IRepository<AsphaltEntity> _repository;

        public AsphaltService(IRepository<AsphaltEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AsphaltEntity>> GetAll(int skip, int limit)
        {
            var asphaltsEntity = new List<AsphaltEntity>();

            try
            {
                IEnumerable<AsphaltEntity> result;

                if (skip == 0 && limit == 0)
                    result = await _repository.Select();
                else
                    result = await _repository.SelectWithPagination(skip, limit);

                if (result != null)
                    return result;
                else
                {
                    var asphaltEntity = new AsphaltEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    asphaltsEntity.Add(asphaltEntity);
                }

                return asphaltsEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<AsphaltEntity>> GetById(int id)
        {
            var ocorrencyDetailEntity = new List<AsphaltEntity>();

            try
            {
                var result = await _repository.SelectById(id);

                if (result != null)
                    ocorrencyDetailEntity.Add(result);
                else
                {
                    var contactEntity = new AsphaltEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    ocorrencyDetailEntity.Add(contactEntity);
                }

                return ocorrencyDetailEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<AsphaltEntity>> GetByOcorrencyId(bool closed, int ocorrencyId)
        {
            var ocorrencyDetailEntity = new List<AsphaltEntity>();

            try
            {
                var result = await _repository.Filter(c => c.EndDate <= DateTime.Today);

                if (result != null)
                    return result;
                else
                {
                    var contactEntity = new AsphaltEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    ocorrencyDetailEntity.Add(contactEntity);
                }

                return ocorrencyDetailEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<AsphaltEntity>> GetFilter(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var predicate = PredicateBuilder.New<AsphaltEntity>(true);

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

        public async Task<AsphaltEntity> Delete(int id)
        {
            var asphaltEntity = new AsphaltEntity();

            try
            {
                var result = await _repository.Delete(id);
                if (result)
                    return asphaltEntity;
                else
                {
                    asphaltEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.SearchHasNoResult,
                        Type = ErrorCode.SearchHasNoResult.ToString(),
                        Message = ErrorCode.SearchHasNoResult.GetDescription()
                    };
                }

                return asphaltEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<AsphaltEntity> Post(AsphaltEntity request)
        {
            var asphaltEntity = new AsphaltEntity();

            try
            {
                var result = await _repository.Insert(request);

                if (result)
                    return asphaltEntity;
                else
                {
                    asphaltEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return asphaltEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<AsphaltEntity> Put(AsphaltEntity request)
        {
            var asphaltEntity = new AsphaltEntity();

            try
            {
                asphaltEntity = await _repository.SelectById(request.Id);
                asphaltEntity.CreatedAt = request.CreatedAt;

                var result = await _repository.Edit(asphaltEntity);

                if (result)
                    return asphaltEntity;
                else
                {
                    asphaltEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return asphaltEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }
    }
}
