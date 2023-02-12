using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Presentation.Model.Response;

namespace TE.BE.City.Presentation.Controllers
{
    [Authorize]
    [Route("api/site/[controller]")]
    public class ReportController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IReportService _reportService;

        private readonly IAsphaltService _asphaltService;
        private readonly ICollectService _collectService;
        private readonly ILightService _lightService;
        private readonly ISewerService _sewerService;
        private readonly ITrashService _trashService;
        private readonly IWaterService _waterService;
        private readonly IPublicServiceService _publicServiceService;

        public ReportController(IReportService reportService, 
            IMapper mapper, 
            IAsphaltService asphaltService,
            ICollectService collectService,
            ILightService lightService,
            ISewerService sewerService,
            ITrashService trashService,
            IWaterService waterService,
            IPublicServiceService publicServiceService
            ) : base()
        {
            _mapper = mapper;
            _reportService = reportService;

            _asphaltService = asphaltService;
            _collectService = collectService;
            _lightService = lightService;
            _sewerService = sewerService;
            _trashService = trashService;
            _waterService = waterService;
            _publicServiceService = publicServiceService;
        }

        /// <summary>
        /// Get all charts data
        /// </summary>
        [HttpGet]
        public async Task<ReportResponseModel> Get([FromQuery]string longStartDate, [FromQuery] string longEndDate)
        {
            DateTime? startDate = long.Parse(longStartDate) > 0 ? new DateTime(long.Parse(longStartDate)) : DateTime.MinValue;
            DateTime? endDate = long.Parse(longEndDate) > 0 ? new DateTime(long.Parse(longEndDate)) : DateTime.MinValue;

            // se funcionar remover nullable e comparacao a null

            ReportResponseModel reportResponseModel = new ReportResponseModel();

            var watersEntity = await _waterService.GetFilter(startDate, endDate);
            reportResponseModel.WaterList = _mapper.Map<List<WaterResponse>>(watersEntity);
            reportResponseModel.CountWater = reportResponseModel.WaterList.Count();

            var lightEntity = await _lightService.GetFilter(startDate, endDate);
            reportResponseModel.LightList = _mapper.Map<List<LightResponse>>(lightEntity);
            reportResponseModel.CountLight = reportResponseModel.LightList.Count();

            var trashEntity = await _trashService.GetFilter(startDate, endDate);
            reportResponseModel.TrashList = _mapper.Map<List<TrashResponse>>(trashEntity);
            reportResponseModel.CountTrash = reportResponseModel.TrashList.Count();

            var collectEntity = await _collectService.GetFilter(startDate, endDate);
            reportResponseModel.CollectList = _mapper.Map<List<CollectResponse>>(collectEntity);
            reportResponseModel.CountCollect = reportResponseModel.CollectList.Count();

            var sewerEntity = await _sewerService.GetFilter(startDate, endDate);
            reportResponseModel.SewerList = _mapper.Map<List<SewerResponse>>(sewerEntity);
            reportResponseModel.CountSewer = reportResponseModel.SewerList.Count();

            var asphaltEntity = await _asphaltService.GetFilter(startDate, endDate);
            reportResponseModel.AsphaltList = _mapper.Map<List<AsphaltResponse>>(asphaltEntity);
            reportResponseModel.CountAsphalt = reportResponseModel.AsphaltList.Count();

            var publicServiceEntity = await _publicServiceService.GetFilter(startDate, endDate);
            reportResponseModel.PublicServiceList = _mapper.Map<List<PublicServiceResponse>>(publicServiceEntity);
            reportResponseModel.CountPublicService = reportResponseModel.PublicServiceList.Count();

            reportResponseModel.Count = 
                reportResponseModel.PublicServiceList.Count() + 
                reportResponseModel.CountAsphalt + 
                reportResponseModel.CountSewer + 
                reportResponseModel.CountCollect +
                reportResponseModel.CountTrash + 
                reportResponseModel.CountLight +
                reportResponseModel.CountWater;

            return reportResponseModel;
        }
    }
}
