using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Domain.Interfaces
{
    public interface ISewerService
    {
        Task<SewerEntity> Post(SewerEntity request);
        Task<SewerEntity> Put(SewerEntity request);
        //Task<SewerEntity> Delete(int id);
        Task<IEnumerable<SewerEntity>> GetAll(int skip, int limit);
        Task<IEnumerable<SewerEntity>> GetClosed(bool closed, int skip, int limit);
        Task <SewerEntity> GetById(int id);
        Task<IEnumerable<SewerEntity>> GetFilter(DateTime? startDate, DateTime? endDate, IsProblem isProblem);
        DataTable GetDataTable(IEnumerable<SewerEntity> asphaltEntities);
        Task<string> GetLocationAddress(int id);
        Task<IEnumerable<SewerEntity>> GetAllByUser(int userId);
    }
}
