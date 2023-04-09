using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using TE.BE.City.Presentation.Model.Response;
using TE.BE.City.Presentation.Model.ViewModel;

namespace TE.BE.City.Presentation.Controllers
{
    [Route("site/[controller]")]
    public class SurveyController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService surveyService, IMapper mapper)
        {
            _surveyService = surveyService;
            _mapper = mapper;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new SurveyViewModel();
            return View(viewModel);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public async Task<IActionResult> Index(SurveyViewModel surveyViewModel)
        {
            surveyViewModel.Error = ValidateModel(surveyViewModel);

            if (surveyViewModel.Error == null)
            {
                var surveyEntity = new SurveyEntity();
                _mapper.Map(surveyViewModel, surveyEntity);
                surveyEntity.CreatedAt = System.DateTime.Now;

                var response = await _surveyService.Post(surveyEntity);

                if (response?.Error == null)
                    surveyViewModel.ShowConfirmation = true;
            }

            return View(surveyViewModel);
        }

        /// <summary>
        /// Internal method to validate model inputs
        /// </summary>
        /// <param name="dataViewState"></param>
        /// <returns></returns>
        private ErrorDetail ValidateModel(SurveyViewModel surveyViewModel)
        {
            ErrorDetail errorDetail = null;

            if (surveyViewModel.Question01 == null ||
                surveyViewModel.Question02 == null ||
                surveyViewModel.Question03 == null ||
                surveyViewModel.Question04 == null ||
                surveyViewModel.Question05 == null)
            {
                errorDetail = new ErrorDetail()
                {
                    Code = 1,
                    Message = "Todas as avaliações devem ser preenchidas.",
                    Type = "Validation"
                };
            }

            return errorDetail;
        }
    }
}
