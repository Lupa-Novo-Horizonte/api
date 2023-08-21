using AutoMapper;
using ClosedXML.Excel;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text.Json;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;
using TE.BE.City.Infra.CrossCutting.Enum;
using TE.BE.City.Presentation.Model.Response;
using TE.BE.City.Presentation.Model.ViewModel;

namespace TE.BE.City.Presentation.Controllers
{
    [Route("site/[controller]")]
    public class HomeController : BaseController
    {
        private IConfiguration _config;
        private readonly IMapper _mapper;

        private readonly IAsphaltService _asphaltService;
        private readonly ICollectService _collectService;
        private readonly ILightService _lightService;
        private readonly ISewerService _sewerService;
        private readonly ITrashService _trashService;
        private readonly IWaterService _waterService;
        private readonly IPublicServiceService _publicServiceService;

        public HomeController(IConfiguration config, IMapper mapper, 
            IAsphaltService asphaltService,
            ICollectService collectService,
            ILightService lightService,
            ISewerService sewerService,
            ITrashService trashService,
            IWaterService waterService,
            IPublicServiceService publicServiceService
            ) : base()
        {
            _config = config;
            _mapper = mapper;

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
        [ApiExplorerSettings(IgnoreApi = true)]
        
        public IActionResult Index()
        {   
            HomeViewModel reportResponseModel = Execute();
            reportResponseModel.ApiKey = _config["GoogleMapsKey"];
            reportResponseModel.Map = Map(reportResponseModel);

            if (HttpContext.Request.QueryString.HasValue && HttpContext.Request.QueryString.Value.Contains("iframe=true"))
                reportResponseModel.IsFrameView = true;

            return View(model: reportResponseModel);
        }

        /// <summary>
        /// Filter all data by date and type
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public IActionResult Filter(DataViewState dataViewState)
        {
            HomeViewModel reportResponseModel = new HomeViewModel(){
                ApiKey = _config["GoogleMapsKey"],
                DataViewState = dataViewState,
                Error = ValidateModel(dataViewState)
            };

            if (reportResponseModel.Error != null)
                return View("Index", model: reportResponseModel);

            reportResponseModel = Execute(dataViewState.DdlIssueType, dataViewState.StartDate, dataViewState.EndDate, (IsProblem)Enum.Parse(typeof(IsProblem),dataViewState.DdlIsProblem, true));
            reportResponseModel.DataViewState = dataViewState;
            reportResponseModel.Map = Map(reportResponseModel);
            reportResponseModel.Chart = Chart(reportResponseModel);

            if (HttpContext.Request.QueryString.HasValue && HttpContext.Request.QueryString.Value.Contains("iframe=true"))
                reportResponseModel.IsFrameView = true;

            return View("Index", model: reportResponseModel);
        }

        /// <summary>
        /// Export file to csv extension
        /// </summary>
        /// <param name="ddlIssueType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("export")]
        public ActionResult Export(string ddlIssueType, string startDate, string endDate, string ddlIsProblem)
        { 
            var reportResponseModel = Execute(ddlIssueType, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), (IsProblem)Enum.Parse(typeof(IsProblem),ddlIsProblem, true));

            var wb = new XLWorkbook();

            var dtAsphalt = _asphaltService.GetDataTable(_mapper.Map<IEnumerable<AsphaltEntity>>(reportResponseModel.AsphaltList));
            wb.Worksheets.Add(dtAsphalt, "Calçadas e Asfalto");

            var dtCollect = _collectService.GetDataTable(_mapper.Map<IEnumerable<CollectEntity>>(reportResponseModel.CollectList));
            wb.Worksheets.Add(dtCollect, "Coleta de Lixo");

            var dtLight = _lightService.GetDataTable(_mapper.Map<IEnumerable<LightEntity>>(reportResponseModel.LightList));
            wb.Worksheets.Add(dtLight, "Iluminação Pública");

            var dtSewer = _sewerService.GetDataTable(_mapper.Map<IEnumerable<SewerEntity>>(reportResponseModel.SewerList));
            wb.Worksheets.Add(dtSewer, "Tratamento de Esgoto");

            var dtTrash = _trashService.GetDataTable(_mapper.Map<IEnumerable<TrashEntity>>(reportResponseModel.TrashList));
            wb.Worksheets.Add(dtTrash, "Limpeza Urbana");

            var dtWater = _waterService.GetDataTable(_mapper.Map<IEnumerable<WaterEntity>>(reportResponseModel.WaterList));
            wb.Worksheets.Add(dtWater, "Água Potável");
                        
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"LupaNH_Report.xlsx");
            }
        }

        /// <summary>
        /// Internal method to structure data for map component.
        /// </summary>
        /// <param name="homeResponseModel"></param>
        /// <returns></returns>
        private MapResponse Map(HomeViewModel homeResponseModel)
        {
            var mapResponse = new MapResponse();
            mapResponse.InitialLatitude = float.Parse(_config["DefaultLocation:latitude"]);
            mapResponse.InitialLongitude = float.Parse(_config["DefaultLocation:longitude"]);
            mapResponse.InitialZoom = float.Parse(_config["DefaultLocation:zoom"]);

            foreach (var item in homeResponseModel.WaterList)
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Type = Infra.CrossCutting.Enum.TypeIssue.Water,
                    IsProblem = item.IsProblem,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Moradia: Água Potável",
                    Description = $"- Possui poço amazônico? {item.HasWell.ToSimNao()} |- Há água encanada? {item.HomeWithWater.ToSimNao()} |- Quantos dias faltam água na semana? {item.WaterMissedInAWeek} |- Alguma obra de saneamento está sendo executada? {item.HasWell.ToSimNao()} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in homeResponseModel.LightList)
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Type = Infra.CrossCutting.Enum.TypeIssue.Light,
                    IsProblem = item.IsProblem,
                    Latitude = string.IsNullOrEmpty(item.Latitude) ? null : float.Parse(item.Latitude),
                    Longitude = string.IsNullOrEmpty(item.Longitude) ? null : float.Parse(item.Longitude),
                    Path = item.Path,
                    Title = "Via: Iluminação Pública",
                    Description = $"- Possui poste? {item.HasLight.ToSimNao()} |- As luzes estão funcionando? {item.IsItWorking.ToSimNao()} |- Há fios elétricos soltos? {item.HasLosesCable.ToSimNao()} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in homeResponseModel.TrashList)
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Type = Infra.CrossCutting.Enum.TypeIssue.Trash,
                    IsProblem = item.IsProblem,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Via: Limpeza Urbana",
                    Description = $"- A prefeitura faz a limpeza? {item.HasRoadcleanUp.ToSimNao()} |- Se sim, qual a frequência semanal? {item.HowManyTimes} |- Existe lixo acumulado? {item.HasAccumulatedTrash.ToSimNao()} |- A prefeitura faz a capinagem? {item.HasAccumulatedTrash.ToSimNao()} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in homeResponseModel.CollectList)
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Type = Infra.CrossCutting.Enum.TypeIssue.Collect,
                    IsProblem = item.IsProblem,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Moradia: Coleta de Lixo",
                    Description = $"- Há coleta de lixo? {item.HasCollect.ToSimNao()} |- Qual a frequência semanal? {item.HowManyTimes} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in homeResponseModel.SewerList)
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Type = Infra.CrossCutting.Enum.TypeIssue.Sewer,
                    IsProblem = item.IsProblem,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Moradia: Tratamento de Esgoto",
                    Description = $"- Há coleta ou tratamento de esgoto? {item.HasHomeSewer.ToSimNao()} |- Possui fossa? {item.HasHomeCesspool.ToSimNao()} |- Alguma obra de saneamento está sendo executada? {item.HasSanitationProject.ToSimNao()} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in homeResponseModel.AsphaltList)
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Type = Infra.CrossCutting.Enum.TypeIssue.Asphalt,
                    IsProblem = item.IsProblem,
                    Latitude = string.IsNullOrEmpty(item.Latitude) ? null : float.Parse(item.Latitude),
                    Longitude = string.IsNullOrEmpty(item.Longitude) ? null : float.Parse(item.Longitude),
                    Path = item.Path,
                    Title = "Via: Calçadas e Asfalto",
                    Description = $"- A via é asfaltada? {item.IsPaved.ToSimNao()} |- A via possui buracos ou crateras? {item.HasHoles.ToSimNao()} |- Há calçadas pavimentadas de acordo com os requisitos municipais? {item.HasPavedSidewalks.ToSimNao()} |- Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            foreach (var item in homeResponseModel.PublicServiceList)
            {
                mapResponse.Regions.Add(new Issues()
                {
                    Type = Infra.CrossCutting.Enum.TypeIssue.PublicService,
                    Latitude = float.Parse(item.Latitude),
                    Longitude = float.Parse(item.Longitude),
                    Title = "Serviços e Referências",
                    Description = $"{item.Service} | Criado em: {item.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"
                });
            }

            mapResponse.RegionsSerialized = JsonSerializer.Serialize(mapResponse.Regions); 

            return mapResponse;
        }

        /// <summary>
        /// Internal method to structure data for map component.
        /// </summary>
        /// <param name="homeResponseModel"></param>
        /// <returns></returns>
        private ChartResponse Chart(HomeViewModel homeResponseModel)
        {
            var chartResponse = new ChartResponse();

            chartResponse.ChartQuantity.Add(new ChartObject { label = "Água Potável", y = homeResponseModel.CountWater });
            chartResponse.ChartQuantity.Add(new ChartObject { label = "Iluminação Pública", y = homeResponseModel.CountLight });
            chartResponse.ChartQuantity.Add(new ChartObject { label = "Limpeza Urbana", y = homeResponseModel.CountTrash });
            chartResponse.ChartQuantity.Add(new ChartObject { label = "Coleta de Lixo", y = homeResponseModel.CountCollect });
            chartResponse.ChartQuantity.Add(new ChartObject { label = "Tratamento de Esgoto", y = homeResponseModel.CountSewer });
            chartResponse.ChartQuantity.Add(new ChartObject { label = "Calçadas e Asfalto", y = homeResponseModel.CountAsphalt });
            chartResponse.ChartQuantitySerialized = JsonSerializer.Serialize(chartResponse.ChartQuantity);

            
            chartResponse.ChartProportion.Add(new ChartProportionObject { label = "Água Potável", y = (homeResponseModel.Count == 0) ? 0 : Convert.ToDouble(homeResponseModel.CountWater) / homeResponseModel.Count });
            chartResponse.ChartProportion.Add(new ChartProportionObject { label = "Iluminação Pública", y = (homeResponseModel.Count == 0) ? 0 : Convert.ToDouble(homeResponseModel.CountLight) / homeResponseModel.Count });
            chartResponse.ChartProportion.Add(new ChartProportionObject { label = "Limpeza Urbana", y = (homeResponseModel.Count == 0) ? 0 : Convert.ToDouble(homeResponseModel.CountTrash) / homeResponseModel.Count });
            chartResponse.ChartProportion.Add(new ChartProportionObject { label = "Coleta de Lixo", y = (homeResponseModel.Count == 0) ? 0 : Convert.ToDouble(homeResponseModel.CountCollect) / homeResponseModel.Count });
            chartResponse.ChartProportion.Add(new ChartProportionObject { label = "Tratamento de Esgoto", y = (homeResponseModel.Count == 0) ? 0 : Convert.ToDouble(homeResponseModel.CountSewer) / homeResponseModel.Count });
            chartResponse.ChartProportion.Add(new ChartProportionObject { label = "Calçadas e Asfalto", y = (homeResponseModel.Count == 0) ? 0 : Convert.ToDouble(homeResponseModel.CountAsphalt) / homeResponseModel.Count });
            chartResponse.ChartProportionSerialized = JsonSerializer.Serialize(chartResponse.ChartProportion);


            chartResponse.ChartTable = new Dictionary<TypeIssue, ChartTable>();
                        
            int countProblemAsphalt = homeResponseModel.AsphaltList.Count(c => c.IsProblem);
            int countNoProblemAslphat = homeResponseModel.AsphaltList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Asphalt, new ChartTable() { 
                ProblemCount = countProblemAsphalt, 
                NoProblemCount = countNoProblemAslphat});

            int countProblemCollect = homeResponseModel.CollectList.Count(c => c.IsProblem);
            int countNoProblemCollect = homeResponseModel.CollectList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Collect, new ChartTable()
            {
                ProblemCount = countProblemCollect,
                NoProblemCount = countNoProblemCollect
            });

            int countProblemLight = homeResponseModel.LightList.Count(c => c.IsProblem);
            int countNoProblemLight = homeResponseModel.LightList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Light, new ChartTable()
            {
                ProblemCount = countProblemLight,
                NoProblemCount = countNoProblemLight
            });

            int countProblemSewer = homeResponseModel.SewerList.Count(c => c.IsProblem);
            int countNoProblemSewer = homeResponseModel.SewerList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Sewer, new ChartTable()
            {
                ProblemCount = countProblemSewer,
                NoProblemCount = countNoProblemSewer
            });

            int countProblemTrash = homeResponseModel.TrashList.Count(c => c.IsProblem);
            int countNoProblemTrash = homeResponseModel.TrashList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Trash, new ChartTable()
            {
                ProblemCount = countProblemTrash,
                NoProblemCount = countNoProblemTrash
            });

            int countProblemWater = homeResponseModel.WaterList.Count(c => c.IsProblem);
            int countNoProblemWater = homeResponseModel.WaterList.Count(c => !c.IsProblem);
            chartResponse.ChartTable.Add(TypeIssue.Water, new ChartTable()
            {
                ProblemCount = countProblemWater,
                NoProblemCount = countNoProblemWater
            });

            chartResponse.ChartTable.Add(TypeIssue.All, new ChartTable()
            {
                ProblemCount = countProblemAsphalt + countProblemCollect + countProblemLight + countProblemSewer + countProblemTrash + countProblemWater,
                NoProblemCount = countNoProblemAslphat + countNoProblemCollect + countNoProblemLight + countNoProblemSewer + countNoProblemTrash + countNoProblemWater
            });

            return chartResponse;
        }

        /// <summary>
        /// Internal method to execute first access and filters
        /// </summary>
        /// <param name="ddlViewTypestring"></param>
        /// <param name="ddlIssueType"></param>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <returns></returns>
        private HomeViewModel Execute(string ddlIssueType = default, DateTime startDate = default, DateTime endDate = default, IsProblem ddlIsProblem = default)
        {
            HomeViewModel reportResponseModel = new HomeViewModel();
            reportResponseModel.ApiKey = _config["GoogleMapsKey"];

            if (ddlIssueType == TypeIssue.All.ToString() || ddlIssueType == TypeIssue.Water.ToString())
            {
                var watersEntity = _waterService.GetFilter(startDate, endDate, ddlIsProblem).Result;
                reportResponseModel.WaterList = _mapper.Map<List<WaterResponse>>(watersEntity);
                reportResponseModel.CountWater = reportResponseModel.WaterList.Count();
            }

            if (ddlIssueType == TypeIssue.All.ToString() || ddlIssueType == TypeIssue.Light.ToString())
            {
                var lightEntity = _lightService.GetFilter(startDate, endDate, ddlIsProblem).Result;
                reportResponseModel.LightList = _mapper.Map<List<LightResponse>>(lightEntity);
                reportResponseModel.CountLight = reportResponseModel.LightList.Count();
            }

            if (ddlIssueType == TypeIssue.All.ToString() || ddlIssueType == TypeIssue.Trash.ToString())
            {
                var trashEntity = _trashService.GetFilter(startDate, endDate, ddlIsProblem).Result;
                reportResponseModel.TrashList = _mapper.Map<List<TrashResponse>>(trashEntity);
                reportResponseModel.CountTrash = reportResponseModel.TrashList.Count();
            }

            if (ddlIssueType == TypeIssue.All.ToString() || ddlIssueType == TypeIssue.Collect.ToString())
            {
                var collectEntity = _collectService.GetFilter(startDate, endDate, ddlIsProblem).Result;
                reportResponseModel.CollectList = _mapper.Map<List<CollectResponse>>(collectEntity);
                reportResponseModel.CountCollect = reportResponseModel.CollectList.Count();
            }

            if (ddlIssueType == TypeIssue.All.ToString() || ddlIssueType == TypeIssue.Sewer.ToString())
            {
                var sewerEntity = _sewerService.GetFilter(startDate, endDate, ddlIsProblem).Result;
                reportResponseModel.SewerList = _mapper.Map<List<SewerResponse>>(sewerEntity);
                reportResponseModel.CountSewer = reportResponseModel.SewerList.Count();
            }

            if (ddlIssueType == TypeIssue.All.ToString() || ddlIssueType == TypeIssue.Asphalt.ToString())
            {
                var asphaltEntity = _asphaltService.GetFilter(startDate, endDate, ddlIsProblem).Result;
                reportResponseModel.AsphaltList = _mapper.Map<List<AsphaltResponse>>(asphaltEntity);
                reportResponseModel.CountAsphalt = reportResponseModel.AsphaltList.Count();
            }

            if (ddlIssueType == TypeIssue.All.ToString() || ddlIssueType == TypeIssue.PublicService.ToString())
            {
                var publicServiceEntity = _publicServiceService.GetFilter(startDate, endDate).Result;
                reportResponseModel.PublicServiceList = _mapper.Map<List<PublicServiceResponse>>(publicServiceEntity);
                reportResponseModel.CountPublicService = reportResponseModel.PublicServiceList.Count();
            }

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
    
        /// <summary>
        /// Internal method to validate model inputs
        /// </summary>
        /// <param name="dataViewState"></param>
        /// <returns></returns>
        private ErrorDetail ValidateModel(DataViewState dataViewState)
        {
            ErrorDetail errorDetail = null;

            if (dataViewState.StartDate > dataViewState.EndDate)
            {
                errorDetail = new ErrorDetail()
                {
                    Code = 1,
                    Message = "Data final não pode ser maior que a data inicial.",
                    Type = "Validation"
                };
            }

            return errorDetail;
        }
    }
}
