using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Domain.Interfaces
{
    public interface ITrashService
    {
        Task<IEnumerable<TrashEntity>> GetAll(int skip, int limit);
        Task<IEnumerable<TrashEntity>> GetClosed(bool closed, int skip, int limit);
        Task<TrashEntity> GetById(int id);
        Task<IEnumerable<TrashEntity>> GetFilter(DateTime? startDate, DateTime? endDate, IsProblem isProblem);
        Task<TrashEntity> Post(TrashEntity request);
        Task<TrashEntity> Put(TrashEntity request);
        Task<TrashEntity> Delete(int id);
        DataTable GetDataTable(IEnumerable<TrashEntity> asphaltEntities);
        Task<string> GetLocationAddress(int id);
    }
}
