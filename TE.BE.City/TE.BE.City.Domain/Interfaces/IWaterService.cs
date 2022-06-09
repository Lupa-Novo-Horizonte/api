using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Domain.Interfaces
{
    public interface IWaterService
    {
        Task<WaterEntity> Post(WaterEntity request);
        Task<bool> Put(WaterEntity request);
        Task<bool> Delete(int id);
        Task<IEnumerable<WaterEntity>> GetAll(WaterEntity request, int skip, int limit);
        Task<WaterEntity> GetById(int id);
        Task<int> GetCount(WaterEntity request);
    }
}
