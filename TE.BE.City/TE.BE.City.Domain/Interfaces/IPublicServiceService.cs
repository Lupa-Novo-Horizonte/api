using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Domain.Interfaces
{
    public interface IPublicServiceService
    {
        Task<PublicServiceEntity> Post(PublicServiceEntity request);
        Task<PublicServiceEntity> Put(PublicServiceEntity request);
        Task<PublicServiceEntity> Delete(int id);
        Task<IEnumerable<PublicServiceEntity>> GetAll(int skip, int limit);
        Task<IEnumerable<PublicServiceEntity>> GetById(int id);
        Task<IEnumerable<PublicServiceEntity>> GetFilter(DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<PublicServiceEntity>> GetByOcorrencyId(bool closed, int ocorrencyId);
    }
}
