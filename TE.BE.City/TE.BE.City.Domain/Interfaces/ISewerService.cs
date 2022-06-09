using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Domain.Interfaces
{
    public interface ISewerService
    {
        Task<SewerEntity> Post(SewerEntity request);
        Task<SewerEntity> Put(SewerEntity request);
        Task<SewerEntity> Delete(int id);
        Task<IEnumerable<SewerEntity>> GetAll(int skip, int limit);
        Task<IEnumerable<SewerEntity>> GetClosed(bool closed, int skip, int limit);
        Task <IEnumerable<SewerEntity>> GetById(int id);
        Task<int> GetCount(bool? closed);
    }
}
