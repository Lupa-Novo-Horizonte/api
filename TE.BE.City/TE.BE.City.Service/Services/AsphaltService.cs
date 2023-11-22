using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Service.Services
{
    public class AsphaltService : IAsphaltService
    {
        private readonly IRepository<AsphaltEntity> _repository;
        private readonly IGoogleMapsWebProvider _googleMapsWebProvider;

        public AsphaltService(IRepository<AsphaltEntity> repository, IGoogleMapsWebProvider googleMapsWebProvider)
        {
            _repository = repository;
            _googleMapsWebProvider = googleMapsWebProvider;
        }

        public async Task<IEnumerable<AsphaltEntity>> GetAll(int skip, int limit)
        {
            var asphaltsEntity = new List<AsphaltEntity>();

            try
            {
                IEnumerable<AsphaltEntity> result;
                var predicate = PredicateBuilder.New<AsphaltEntity>(true);
                predicate.And(model => model.StatusId == 1);

                if (skip == 0 && limit == 0)
                    result = await _repository.Filter(predicate);
                else
                    result = await _repository.FilterWithPagination(predicate, skip, limit);

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

        public async Task<AsphaltEntity> GetById(int id)
        {
            try
            {
                var result = await _repository.SelectById(id);

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
                    return contactEntity;
                }
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public DataTable GetDataTable(IEnumerable<AsphaltEntity> asphaltEntities)
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
            column.ColumnName = "Trecho";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "A via é asfaltada?";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "A via possui buracos ou crateras?";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Há calçadas pavimentadas de acordo com os requisitos municipais?";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Criado em";
            dataTable.Columns.Add(column);
            
            column = new DataColumn();
            column.ColumnName = "Ocorrência de problema?";
            dataTable.Columns.Add(column);

            foreach (var entity in asphaltEntities)
            {
                var row = dataTable.NewRow();
                row[0] = entity.Id.ToString();
                row[1] = entity.Longitude?.ToString();
                row[2] = entity.Latitude?.ToString();
                row[3] = entity.Path?.ToString();
                row[4] = entity.IsPaved.ToSimNao();
                row[5] = entity.HasHoles.ToSimNao();
                row[6] = entity.HasPavedSidewalks.ToSimNao();
                row[7] = entity.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss");
                row[8] = entity.IsProblem.ToSimNao();

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public async Task<IEnumerable<AsphaltEntity>> GetFilter(DateTime? startDate, DateTime? endDate, IsProblem isProblem)
        {
            try
            {
                var predicate = PredicateBuilder.New<AsphaltEntity>(true);
                predicate.And(model => model.StatusId == 1);

                if (startDate != null && startDate > DateTime.MinValue)
                    predicate.And(model => model.CreatedAt.Date >= startDate);
                if (endDate != null && endDate > DateTime.MinValue)
                    predicate.And(model => model.CreatedAt.Date <= endDate);

                var result = await _repository.Filter(predicate);
                
                return isProblem switch
                {
                    IsProblem.All => result,
                    IsProblem.Problem => result.Where(c => c.IsProblem),
                    _ => result.Where(c => c.IsProblem == false)
                };
            }
            catch
            {
                throw new Exception();
            }
        }

        /*public async Task<AsphaltEntity> Delete(int id)
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
        }*/

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
                asphaltEntity.StatusId = request.StatusId;
                
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

        public async Task<string> GetLocationAddress(int id)
        {
            string address = string.Empty;
            try
            {
                var item = await GetById(id);
                
                if(! string.IsNullOrEmpty(item.Latitude) && ! string.IsNullOrEmpty(item.Longitude))
                    address = await _googleMapsWebProvider.GetAddress(item.Latitude, item.Longitude);
                
                return address;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<AsphaltEntity>> GetAllByUser(int userId)
        {
            try
            {
                var predicate = PredicateBuilder.New<AsphaltEntity>(true);
                predicate.And(model => model.UserId == userId);

                return await _repository.Filter(predicate);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
