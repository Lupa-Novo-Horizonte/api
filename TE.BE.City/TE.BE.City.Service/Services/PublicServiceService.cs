using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting.Enum;
using TE.BE.City.Infra.CrossCutting;
using LinqKit;
using System.Data;

namespace TE.BE.City.Service.Services
{
    public class PublicServiceService : IPublicServiceService
    {
        private readonly IRepository<PublicServiceEntity> _repository;

        public PublicServiceService(IRepository<PublicServiceEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PublicServiceEntity>> GetAll(int skip, int limit)
        {
            var publicServicesEntity = new List<PublicServiceEntity>();

            try
            {
                IEnumerable<PublicServiceEntity> result;

                if (skip == 0 && limit == 0)
                    result = await _repository.Select();
                else
                    result = await _repository.SelectWithPagination(skip, limit);

                if (result != null)
                    return result;
                else
                {
                    var publicServiceEntity = new PublicServiceEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    publicServicesEntity.Add(publicServiceEntity);
                }

                return publicServicesEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<PublicServiceEntity>> GetById(int id)
        {
            var ocorrencyDetailEntity = new List<PublicServiceEntity>();

            try
            {
                var result = await _repository.SelectById(id);

                if (result != null)
                    ocorrencyDetailEntity.Add(result);
                else
                {
                    var publicServiceEntity = new PublicServiceEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    ocorrencyDetailEntity.Add(publicServiceEntity);
                }

                return ocorrencyDetailEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<PublicServiceEntity>> GetByOcorrencyId(bool closed, int ocorrencyId)
        {
            var ocorrencyDetailEntity = new List<PublicServiceEntity>();

            try
            {
                var result = await _repository.Filter(c => c.EndDate <= DateTime.Today);

                if (result != null)
                    return result;
                else
                {
                    var publicServiceEntity = new PublicServiceEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    ocorrencyDetailEntity.Add(publicServiceEntity);
                }

                return ocorrencyDetailEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<PublicServiceEntity>> GetFilter(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var predicate = PredicateBuilder.New<PublicServiceEntity>(true);

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

        public async Task<PublicServiceEntity> Delete(int id)
        {
            var publicServiceEntity = new PublicServiceEntity();

            try
            {
                var result = await _repository.Delete(id);
                if (result)
                    return publicServiceEntity;
                else
                {
                    publicServiceEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.SearchHasNoResult,
                        Type = ErrorCode.SearchHasNoResult.ToString(),
                        Message = ErrorCode.SearchHasNoResult.GetDescription()
                    };
                }

                return publicServiceEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<PublicServiceEntity> Post(PublicServiceEntity request)
        {
            var publicServiceEntity = new PublicServiceEntity();

            try
            {
                var result = await _repository.Insert(request);

                if (result)
                    return publicServiceEntity;
                else
                {
                    publicServiceEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return publicServiceEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<PublicServiceEntity> Put(PublicServiceEntity request)
        {
            var publicServiceEntity = new PublicServiceEntity();

            try
            {
                publicServiceEntity = await _repository.SelectById(request.Id);
                publicServiceEntity.CreatedAt = request.CreatedAt;
                publicServiceEntity.Latitude = request.Latitude;
                publicServiceEntity.Longitude = request.Longitude;
                publicServiceEntity.Service = request.Service;

                var result = await _repository.Edit(publicServiceEntity);

                if (result)
                    return publicServiceEntity;
                else
                {
                    publicServiceEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return publicServiceEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public DataTable GetDataTable(IEnumerable<PublicServiceEntity> asphaltEntities)
        {
            DataTable dataTable = new DataTable();
            DataColumn column = null;

            column = new DataColumn();
            column.ColumnName = "ID";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Latitude";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Longitude";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Serviço?";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Criado em";
            dataTable.Columns.Add(column);

            foreach (var entity in asphaltEntities)
            {
                var row = dataTable.NewRow();
                row[0] = entity.Id.ToString();
                row[1] = entity.Longitude.ToString();
                row[2] = entity.Latitude.ToString();
                row[3] = entity.Service.ToString();
                row[4] = entity.CreatedAt.ToShortDateString();

                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
    }
}
