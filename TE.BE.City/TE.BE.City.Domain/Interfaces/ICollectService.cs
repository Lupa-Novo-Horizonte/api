using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Domain.Interfaces
{
    public interface ICollectService
    {
        Task<CollectEntity> Post(CollectEntity request);
        Task<CollectEntity> Put(CollectEntity request);
        Task<CollectEntity> Delete(int id);
        Task<IEnumerable<CollectEntity>> GetAll(int skip, int limit);
        Task<IEnumerable<CollectEntity>> GetById(int id);
        Task<IEnumerable<CollectEntity>> GetFilter(DateTime? startDate, DateTime? endDate);
        DataTable GetDataTable(IEnumerable<CollectEntity> asphaltEntities);
    }
}
