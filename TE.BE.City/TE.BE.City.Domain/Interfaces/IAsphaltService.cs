using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Domain.Interfaces
{
    public interface IAsphaltService
    {
        Task<AsphaltEntity> Post(AsphaltEntity request);
        Task<AsphaltEntity> Put(AsphaltEntity request);
        //Task<AsphaltEntity> Delete(int id);
        Task<IEnumerable<AsphaltEntity>> GetAll(int skip, int limit);
        Task<AsphaltEntity> GetById(int id);
        Task<IEnumerable<AsphaltEntity>> GetFilter(DateTime? startDate, DateTime? endDate, IsProblem isProblem);
        DataTable GetDataTable(IEnumerable<AsphaltEntity> asphaltEntities);
        Task<string> GetLocationAddress(int id);
    }
}
