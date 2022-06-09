using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Domain.Interfaces
{
    public interface IReportService
    {
        //Task<Dictionary<string, int>> GetNumberOcorrencyXtype();
        Task<Dictionary<string, int>> GetNumberOcorrencyXday();
        Task<Dictionary<string, Dictionary<string, int>>> NumberOcorrencyXstatusXday();
        Task<List<WaterEntity>> LastIncomes();
        Task<List<WaterEntity>> LastUpdates();
    }
}
