//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Threading.Tasks;
//using TE.BE.City.Domain.Entity;
//using TE.BE.City.Domain.Interfaces;
//using TE.BE.City.Presentation.Model.Request;
//using TE.BE.City.Presentation.Model.Response;

//namespace TE.BE.City.Presentation.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    public class ReportController :  BaseController
//    {
//        private readonly IMapper _mapper;
//        private readonly IReportService _reportService;

//        public ReportController(IReportService reportService, IMapper mapper) : base()
//        {
//            _mapper = mapper;
//            _reportService = reportService;
//        }

//        /// <summary>
//        /// Get all charts data
//        /// </summary>
//        [HttpGet]
//        public async Task<ReportResponseModel> Get()
//        {
//            ReportResponseModel reportResponseModel = new ReportResponseModel();

//            reportResponseModel.NumberOcorrencyXday = await _reportService.GetNumberOcorrencyXday();
//            reportResponseModel.NumberOcorrencyXstatusXday = await _reportService.NumberOcorrencyXstatusXday();

//            _mapper.Map(await _reportService.LastIncomes(), reportResponseModel.LastIncomes);
//            _mapper.Map(await _reportService.LastUpdates(), reportResponseModel.LastUpdates);
            
//            return reportResponseModel;
//        }
//    }
//}
