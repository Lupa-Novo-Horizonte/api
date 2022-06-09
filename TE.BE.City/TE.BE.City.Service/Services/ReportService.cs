using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using TE.BE.City.Infra.CrossCutting.Enum;
using System.Linq.Expressions;
using LinqKit;

namespace TE.BE.City.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<WaterEntity> _orderRepository;
        private readonly IRepository<CollectEntity> _ocorrencyRepository;
        private readonly IRepository<AsphaltEntity> _ocorrencyDetailRepository;
        private readonly IRepository<StatusEntity> _StatusRepository;
        
        /// <summary>
        /// Iniciate my dependy injection
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="token"></param>
        public ReportService(IRepository<WaterEntity> orderRepository,
            IRepository<CollectEntity> ocorrencyRepository,
            IRepository<AsphaltEntity> ocorrencyDetailRepository,
            IRepository<StatusEntity> StatusRepository)
        {
            _orderRepository = orderRepository;
            _ocorrencyRepository = ocorrencyRepository;
            _ocorrencyDetailRepository = ocorrencyDetailRepository;
            _StatusRepository = StatusRepository;
        }

        public async Task<Dictionary<string, int>> GetNumberOcorrencyXday()
        {
            var dictionarynumberOcorrencyXday = new Dictionary<string, int>();

            try
            {
                var result = await _orderRepository.Select();
                dictionarynumberOcorrencyXday = result.GroupBy(c => c.CreatedAt.Date).ToDictionary(c => c.Key.ToShortDateString(), c => c.ToList().Count);

                return dictionarynumberOcorrencyXday;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        //public async Task<Dictionary<string, int>> GetNumberOcorrencyXtype()
        //{
        //    var numberOcorrencyXtype = new Dictionary<string, int>();

        //    try
        //    {
        //        var result = await _orderRepository.Select();
        //        var queryResult = result.GroupBy(c => c.OcorrencyId).ToDictionary(c => c.Key.ToString(), c => c.ToList().Count);

        //        // get ocorrency name
        //        foreach (var item in queryResult) {
        //            var ocorrency = await _ocorrencyRepository.SelectById(Convert.ToInt32(item.Key));
        //            numberOcorrencyXtype.Add(ocorrency.Name, item.Value);
        //        }

        //        return numberOcorrencyXtype;
        //    }
        //    catch (ExecptionHelper.ExceptionService ex)
        //    {
        //        throw new ExecptionHelper.ExceptionService(ex.Message);
        //    }
        //}

        public async Task<Dictionary<string, Dictionary<string, int>>> NumberOcorrencyXstatusXday()
        {
            var numberOcorrencyXstatusXday = new Dictionary<string, Dictionary<string, int>>();

            try
            {
                var result = await _orderRepository.Select();

                foreach (var item in result)
                {
                    var status = await _StatusRepository.SelectById(item.StatusId);
                    item.Status = new StatusEntity()
                    {
                        Id = item.StatusId,
                        Name = status.Name
                    };
                }

                var queryResult = result.GroupBy(c => c.CreatedAt.Date)
                    .ToDictionary(c => c.Key.ToShortDateString(), c => c.GroupBy(c => c.Status.Name)
                    .ToDictionary(c => c.Key.ToString(), c => c.ToList().Count));

                return numberOcorrencyXstatusXday = queryResult;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<List<WaterEntity>> LastIncomes()
        {
            var result = await _orderRepository.Select();

            if (result != null)
            {
                // get order status
                foreach (var item in result.ToList())
                    item.Status = await _StatusRepository.SelectById(item.StatusId);

                return result.OrderByDescending(c => c.CreatedAt.Date).TakeLast(5).ToList();
            }
            else
            {
                var ordersEntity = new List<WaterEntity>();
                var orderEntity = new WaterEntity()
                {
                    Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.SearchHasNoResult,
                        Type = ErrorCode.SearchHasNoResult.ToString(),
                        Message = ErrorCode.SearchHasNoResult.GetDescription()
                    }
                };
                ordersEntity.Add(orderEntity);
                return ordersEntity;
            }
        }

        public async Task<List<WaterEntity>> LastUpdates()
        {
            var result = await _orderRepository.Select();

            return result.OrderByDescending(c => c.CreatedAt.Date).TakeLast(5).ToList();
        }
    }
}