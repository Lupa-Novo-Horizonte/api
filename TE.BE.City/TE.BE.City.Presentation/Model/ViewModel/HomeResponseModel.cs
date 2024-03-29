﻿using System.Collections.Generic;
using TE.BE.City.Infra.CrossCutting;
using TE.BE.City.Presentation.Model.Response;

namespace TE.BE.City.Presentation.Model.ViewModel
{
    /// <summary>
    /// Model responsable for itens on the user interface. It represent the user interface. Not related to the database tables or domain layer.
    /// </summary>
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            AsphaltList = new List<AsphaltResponse>();
            WaterList = new List<WaterResponse>();
            CollectList = new List<CollectResponse>();
            TrashList = new List<TrashResponse>();
            LightList = new List<LightResponse>();
            SewerList = new List<SewerResponse>();
            PublicServiceList = new List<PublicServiceResponse>();
            DataViewState = new DataViewState();
            Map = new MapResponse();
            Chart = new ChartResponse();
            News = new NewsViewModel();
        }
        public List<AsphaltResponse> AsphaltList { get; set; }
        public List<WaterResponse> WaterList { get; set; }
        public List<CollectResponse> CollectList { get; set; }
        public List<TrashResponse> TrashList { get; set; }
        public List<LightResponse> LightList { get; set; }
        public List<SewerResponse> SewerList { get; set; }
        public List<PublicServiceResponse> PublicServiceList { get; set; }

        public int Count { get; set; }
        public int CountAsphalt { get; set; }
        public int CountWater { get; set; }
        public int CountCollect { get; set; }
        public int CountTrash { get; set; }
        public int CountLight { get; set; }
        public int CountSewer { get; set; }
        public int CountPublicService { get; set; }
        public bool IsFrameView { get; set; }
        public string ApiKey { get; set; }
        public DataViewState DataViewState { get; set; }
        public ErrorDetail Error { get; set; }

        public MapResponse Map { get; set; }
        public ChartResponse Chart { get; set; }
        public NewsViewModel News { get; set; }
    }
}
