using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Domain.Interfaces
{
    public interface ILightService
    {
        Task<LightEntity> Post(LightEntity request);
        Task<LightEntity> Put(LightEntity request);
        Task<LightEntity> Delete(int id);
        Task<LightEntity> Get();
        Task<IEnumerable<LightEntity>> GetAll(int skip, int limit);
        Task<IEnumerable<LightEntity>> GetFilter(DateTime? startDate, DateTime? endDate, IsProblem isProblem);
        DataTable GetDataTable(IEnumerable<LightEntity> asphaltEntities);
        Task<string> GetLocationAddress(int id);
    }
}
