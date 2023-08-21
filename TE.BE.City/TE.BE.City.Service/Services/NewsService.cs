using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Service.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsDomain<NewsPriorityEntity> _newsDomain;
        private readonly IRepository<NewsPriorityEntity> _repositoryNewsPriority;
        private readonly IRepository<NewsTextEntity> _repositoryNewsText;
        private readonly IOpenAIWebProvider _openAiWebProvider;

        public NewsService(INewsDomain<NewsPriorityEntity> newsDomain, 
            IRepository<NewsPriorityEntity> repositoryNewsPriority,
            IRepository<NewsTextEntity> repositoryNewsText,
            IOpenAIWebProvider openAiWebProvider)
        {
            _newsDomain = newsDomain;
            _repositoryNewsPriority = repositoryNewsPriority;
            _repositoryNewsText = repositoryNewsText;
            _openAiWebProvider = openAiWebProvider;
        }
      
        public async Task<NewsPriorityEntity> Next()
        {
            var newsPriorityList = await _repositoryNewsPriority.Select();

            foreach (var newsPriorityItem in newsPriorityList) {
                _newsDomain.Add(newsPriorityItem, newsPriorityItem.Weight);
            }
            
            return  _newsDomain.Next();
        }

        public async Task<NewsTextEntity> Text(GenerativeTool generativeTool)
        {
            var newsTextList = await _repositoryNewsText.Filter(c => c.GenerativeTool.ToLower() == generativeTool.ToString().ToLower());
            
            int index = Random.Shared.Next(newsTextList.Count());

            return newsTextList.ElementAtOrDefault(index);
        }
        
        public async Task<string> GenerateNewsRecomendation(string subject)
        {
            try
            {
                return await _openAiWebProvider.GenerateNewsRecomendation(subject);
            }
            catch(Exception ex)
            {
                return "Limite de cotas para chatGPT esgotadas. Contate o administrador.";
            }
        }

        public async Task<bool> UpdatePriorityTable(List<NewsPriorityEntity> listNewsPriorityEntities)
        {
            var listPriority = await _repositoryNewsPriority.Select();
            await _repositoryNewsPriority.DeleteRange(listPriority);
            await _repositoryNewsPriority.InsertRange(listNewsPriorityEntities);

            return true;
        }
    }
}
