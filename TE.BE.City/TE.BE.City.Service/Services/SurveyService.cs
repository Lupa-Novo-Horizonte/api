using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Service.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly IRepository<SurveyEntity> _repository;

        public SurveyService(IRepository<SurveyEntity> repository)
        {
            _repository = repository;
        }

        public async Task<SurveyEntity> Post(SurveyEntity request)
        {
            var surveyEntity = new SurveyEntity();

            try
            {
                var result = await _repository.Insert(request);

                if (result)
                    return surveyEntity;
                else
                {
                    surveyEntity.Error = new ErrorDetail()
                    {
                        Code = (int)ErrorCode.SurveyError,
                        Type = ErrorCode.SurveyError.ToString(),
                        Message = ErrorCode.SurveyError.GetDescription()
                    };
                }

                return surveyEntity;
            }
            catch (ExecptionHelper.ExceptionService ex)
            {
                throw new ExecptionHelper.ExceptionService(ex.Message);
            }
        }
    }
}
