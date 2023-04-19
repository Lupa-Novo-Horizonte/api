﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using System.Linq;
using TE.BE.City.Infra.CrossCutting.Enum;
using LinqKit;
using System.Data;

namespace TE.BE.City.Service.Services
{
    public class TrashService : ITrashService
    {
        private readonly IRepository<TrashEntity> _repository;

        public TrashService(IRepository<TrashEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TrashEntity>> GetAll(int skip, int limit)
        {
            var newsEntity = new List<TrashEntity>();

            try
            {
                IEnumerable<TrashEntity> result;

                if (skip == 0 && limit == 0)
                    result = await _repository.Select();
                else
                    result = await _repository.SelectWithPagination(skip, limit);

                if (result != null)
                    return result;
                else
                {
                    var contactEntity = new TrashEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    newsEntity.Add(contactEntity);
                }

                return newsEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<TrashEntity>> GetById(int id)
        {
            var newsEntity = new List<TrashEntity>();

            try
            {
                var result = await _repository.SelectById(id);

                if (result != null)
                    newsEntity.Add(result);
                else
                {
                    var contactEntity = new TrashEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    newsEntity.Add(contactEntity);
                }

                return newsEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<TrashEntity>> GetClosed(bool closed, int skip, int limit)
        {
            var newsEntity = new List<TrashEntity>();

            try
            {
                IEnumerable<TrashEntity> result;

                if (skip == 0 && limit == 0)
                    result = await _repository.Filter(c => c.EndDate <= DateTime.Today);
                else
                    result = await _repository.FilterWithPagination(c => c.EndDate <= DateTime.Today, skip, limit);

                if (result != null)
                    return result;
                else
                {
                    var contactEntity = new TrashEntity()
                    {
                        Error = new ErrorDetail()
                        {
                            Code = (int)ErrorCode.SearchHasNoResult,
                            Type = ErrorCode.SearchHasNoResult.ToString(),
                            Message = ErrorCode.SearchHasNoResult.GetDescription()
                        }
                    };
                    newsEntity.Add(contactEntity);
                }

                return newsEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<IEnumerable<TrashEntity>> GetFilter(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var predicate = PredicateBuilder.New<TrashEntity>(true);

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

        public async Task<TrashEntity> Delete(int id)
        {
            var trashEntity = new TrashEntity();

            try
            {
                var result = await _repository.Delete(id);
                if (result)
                    return trashEntity;
                else
                {
                    trashEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.SearchHasNoResult,
                        Type = ErrorCode.SearchHasNoResult.ToString(),
                        Message = ErrorCode.SearchHasNoResult.GetDescription()
                    };
                }

                return trashEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<TrashEntity> Post(TrashEntity request)
        {
            var trashEntity = new TrashEntity();

            try
            {
                var result = await _repository.Insert(request);

                if (result)
                    return trashEntity;
                else
                {
                    trashEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return trashEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public async Task<TrashEntity> Put(TrashEntity request)
        {
            var trashEntity = new TrashEntity();

            try
            {
                trashEntity = await _repository.SelectById(request.Id);
                trashEntity.Longitude = request.Longitude;
                trashEntity.Latitude = request.Latitude;
                trashEntity.HasRoadCleanUp = request.HasRoadCleanUp;
                trashEntity.HowManyTimes = request.HowManyTimes;
                trashEntity.HasAccumulatedTrash = request.HasAccumulatedTrash;
                trashEntity.HasLandWeeding = request.HasLandWeeding;
                trashEntity.CreatedAt = DateTime.Now.ToUniversalTime();
                trashEntity.UserId = request.UserId;
                trashEntity.StatusId = request.StatusId;

                var result = await _repository.Edit(trashEntity);

                if (result)
                    return trashEntity;
                else
                {
                    trashEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.InsertContactFail,
                        Type = ErrorCode.InsertContactFail.ToString(),
                        Message = ErrorCode.InsertContactFail.GetDescription()
                    };
                }

                return trashEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }

        public DataTable GetDataTable(IEnumerable<TrashEntity> asphaltEntities)
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
            column.ColumnName = "A prefeitura faz a limpeza?";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Qual a frequência?";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Existe lixo acumulado?";
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
                row[3] = entity.HasRoadCleanUp.ToSimNao();
                row[4] = entity.HowManyTimes.ToString();
                row[5] = entity.HasAccumulatedTrash.ToSimNao();
                row[6] = entity.CreatedAt.ToShortDateString();

                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
    }
}
