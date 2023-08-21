using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using System.Linq;
using TE.BE.City.Infra.CrossCutting.Enum;
using System.Data;
using LinqKit;

namespace TE.BE.City.Service.Services
{
    public class LightService : ILightService
    {
        private readonly IRepository<LightEntity> _repository;
        private readonly IGoogleMapsWebProvider _googleMapsWebProvider;

        public LightService(IRepository<LightEntity> repository, IGoogleMapsWebProvider googleMapsWebProvider)
        {
            _repository = repository;
            _googleMapsWebProvider = googleMapsWebProvider;
        }

        public async Task<IEnumerable<LightEntity>> GetAll(int skip, int limit)
        {
            var lightsEntity = new List<LightEntity>();

            try
            {
                IEnumerable<LightEntity> result;
                var predicate = PredicateBuilder.New<LightEntity>(true);
                predicate.And(model => model.StatusId == 1);

                if (skip == 0 && limit == 0)
                    result = await _repository.Filter(predicate);
                else
                    result = await _repository.FilterWithPagination(predicate, skip, limit);

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

        /*public async Task<LightEntity> Get()
        {
            try
            {
                var predicate = PredicateBuilder.New<LightEntity>(true);
                predicate.And(model => model.StatusId == 1);
                
                var result = await _repository.Filter(predicate);
                return result.FirstOrDefault();
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }*/

        public async Task<LightEntity> GetById(int id)
        {
            try
            {
                var result = await _repository.SelectById(id);

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
                    return lightEntity;
                }
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }
        
        /*public async Task<LightEntity> Delete(int id)
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
        }*/

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

        public async Task<IEnumerable<LightEntity>> GetFilter(DateTime? startDate, DateTime? endDate, IsProblem isProblem)
        {
            try
            {
                var predicate = PredicateBuilder.New<LightEntity>(true);
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

        public DataTable GetDataTable(IEnumerable<LightEntity> asphaltEntities)
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
            column.ColumnName = "Possui poste?";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "As luzes estão funcionando?";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Há fios elétricos soltos?";
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
                row[4] = entity.HasLight.ToSimNao();
                row[5] = entity.IsItWorking.ToSimNao();
                row[6] = entity.HasLosesCable.ToSimNao();
                row[7] = entity.CreatedAt.ToShortDateString();
                row[8] = entity.IsProblem.ToSimNao();

                dataTable.Rows.Add(row);
            }
            return dataTable;
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

        public async Task<IEnumerable<LightEntity>> GetAllByUser(int userId)
        {
            try
            {
                var predicate = PredicateBuilder.New<LightEntity>(true);
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
