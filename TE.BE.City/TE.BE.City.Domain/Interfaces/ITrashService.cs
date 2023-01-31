using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Domain.Interfaces
{
    public interface ITrashService
    {
        Task<IEnumerable<TrashEntity>> GetAll(int skip, int limit);
        Task<IEnumerable<TrashEntity>> GetClosed(bool closed, int skip, int limit);
        Task<IEnumerable<TrashEntity>> GetById(int id);
        Task<IEnumerable<TrashEntity>> GetFilter(DateTime? startDate, DateTime? endDate);
        Task<TrashEntity> Post(TrashEntity request);
        Task<TrashEntity> Put(TrashEntity request);
        Task<TrashEntity> Delete(int id);
    }
}
