using System.Threading.Tasks;

namespace TE.BE.City.Domain.Interfaces
{
    public interface IOpenAIWebProvider
    {
        public Task<string> GenerateNewsRecomendation(string subject);
    }
}