using System.Collections.Generic;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Domain.Interfaces
{
    public interface INewsService
    {
        public Task<NewsPriorityEntity> Next();
        public Task<NewsTextEntity> Text(GenerativeTool generativeTool);
        public Task<string> GenerateNewsRecomendation(string subject);
        public Task<bool> UpdatePriorityTable(List<NewsPriorityEntity> listNewsPriorityEntities);
    }
}
