using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Domain.Interfaces
{
    public interface ISurveyService
    {
        Task<SurveyEntity> Post(SurveyEntity request);
    }
}
