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
    public class SewerService : ISewerService
    {
        private readonly IRepository<SewerEntity> _repository;
        private readonly IGoogleMapsWebProvider _googleMapsWebProvider;
        
        public SewerService(IRepository<SewerEntity> repository, IGoogleMapsWebProvider googleMapsWebProvider)
        {
            _repository = repository;
            _googleMapsWebProvider = googleMapsWebProvider;
        }
        
        /*public async Task<SewerEntity> Delete(int id)
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
        }*/

        public async Task<IEnumerable<SewerEntity>> GetAll(int skip, int limit)
        {
            var contactsEntity = new List<SewerEntity>();

            try
            {
                IEnumerable<SewerEntity> result;
                var predicate = PredicateBuilder.New<SewerEntity>(true);
                predicate.And(model => model.StatusId == 1);

                if (skip == 0 && limit == 0)
                    result = await _repository.Filter(predicate);
                else
                    result = await _repository.FilterWithPagination(predicate, skip, limit);

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

        public async Task<SewerEntity> GetById(int id)
        {
            try
            {
                var result =  await _repository.SelectById(id);

                if(result != null)
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
                    return contactEntity;
                }
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

        public async Task<IEnumerable<SewerEntity>> GetFilter(DateTime? startDate, DateTime? endDate, IsProblem isProblem)
        {
            try
            {
                var predicate = PredicateBuilder.New<SewerEntity>(true);
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

        public DataTable GetDataTable(IEnumerable<SewerEntity> asphaltEntities)
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
            column.ColumnName = "Há coleta ou tratamento de esgoto?";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Possui fossa?";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Alguma obra de saneamento está sendo executada?";
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
                row[1] = entity.Longitude.ToString();
                row[2] = entity.Latitude.ToString();
                row[3] = entity.HasHomeSewer.ToSimNao();
                row[4] = entity.HasHomeCesspool.ToSimNao();
                row[5] = entity.HasSanitationProject.ToSimNao();
                row[6] = entity.CreatedAt.ToShortDateString();
                row[7] = entity.IsProblem.ToSimNao();

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
    }
}
