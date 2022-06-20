using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Service.Services
{
    public class WaterService : IWaterService
    {
        private readonly IRepository<WaterEntity> _waterRepository;
        private readonly IRepository<StatusEntity> _StatusRepository;
        
        /// <summary>
        /// Iniciate my dependy injection
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="token"></param>
        public WaterService(IRepository<WaterEntity> orderRepository, IRepository<StatusEntity> StatusRepository)
        {
            _waterRepository = orderRepository;
            _StatusRepository = StatusRepository;
        }

        /// <summary>
        /// Insert new item on the database
        /// </summary>
        /// <param name="request"></param>
        public async Task<WaterEntity> Post(WaterEntity request)
        {
            try
            {
                var waterEntity = new WaterEntity();

                var result = await _waterRepository.Insert(request);

                if (result)
                    return waterEntity;
                else
                {
                    waterEntity.Error = new ErrorDetail()
                    {
                        Code = (int) ErrorCode.CreateIssueFail,
                        Type = ErrorCode.CreateIssueFail.ToString(),
                        Message = ErrorCode.CreateIssueFail.GetDescription()
                    };
                    
                    return waterEntity;
                }
            }
            catch (Exception ex)
            {
                return new WaterEntity()
                {
                    Error = new ErrorDetail() { Code = (int)ErrorCode.CreateIssueFail, Message = ex.Message, Type = ex.InnerException.Message }
                };
            }
        }

        /// <summary>
        /// Get all itens
        /// </summary>
        public async Task<IEnumerable<WaterEntity>> GetAll(int skip, int limit)
        {
            var waterEntity = new List<WaterEntity>();

            try
            {
                IEnumerable<WaterEntity> result;

                if (skip == 0 && limit == 0)
                    result = await _waterRepository.Select();
                else
                    result = await _waterRepository.SelectWithPagination(skip, limit);

                if (result != null)
                    return result;
                else
                {
                    var orderEntity = new WaterEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    waterEntity.Add(orderEntity);
                }

                return waterEntity;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get a item by id
        /// </summary>
        public async Task<WaterEntity> GetById(int id)
        {
            try
            {
                var result = await _waterRepository.SelectById(id);

                if (result != null)
                {
                    // get order status
                    result.Status = await _StatusRepository.SelectById(result.StatusId);

                    return result;
                }
                else
                {
                    var orderEntity = new WaterEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    return orderEntity;
                }
            }
            catch (Exception ex)
            {
                return new WaterEntity()
                {
                    Error = new ErrorDetail() { Code = (int)ErrorCode.CreateIssueFail, Message = ex.Message, Type = ex.InnerException.Message }
                };
            }
        }

        public async Task<int> GetCount(WaterEntity request)
        {
            try
            {
                var predicate = PredicateBuilder.New<WaterEntity>(true);

                if (request.Id > 0)
                    predicate.And(model => model.Id == request.Id);                
                if (request.StatusId > 0)
                    predicate.And(model => model.StatusId == request.StatusId);
                if (request.CreatedAt > DateTime.MinValue)
                    predicate.And(model => model.CreatedAt.Date >= request.CreatedAt.Date);
                if (request.EndDate > DateTime.MinValue)
                    predicate.And(model => model.CreatedAt.Date <= request.EndDate.Date);

                var result = await _waterRepository.Filter(predicate);
                return result.Count();
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Update an item by entity
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> Put(WaterEntity request)
        {
            try
            {
                return await _waterRepository.Edit(request);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Delete item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(int id)
        {
            try
            {
                return await _waterRepository.Delete(id);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}