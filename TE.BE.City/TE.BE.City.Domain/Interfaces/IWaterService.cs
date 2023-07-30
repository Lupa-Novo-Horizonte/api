using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Domain.Interfaces
{
    public interface IWaterService
    {
        Task<WaterEntity> Post(WaterEntity request);
        Task<bool> Put(WaterEntity request);
        Task<bool> Delete(int id);
        Task<IEnumerable<WaterEntity>> GetAll(int skip, int limit);
        Task<WaterEntity> GetById(int id);
        Task<IEnumerable<WaterEntity>> GetFilter(DateTime? startDate, DateTime? endDate, IsProblem isProblem);
        DataTable GetDataTable(IEnumerable<WaterEntity> asphaltEntities);
        Task<string> GetLocationAddress(int id);
    }
}
