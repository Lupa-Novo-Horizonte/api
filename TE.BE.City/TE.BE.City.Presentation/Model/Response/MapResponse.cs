using System.Collections.Generic;

namespace TE.BE.City.Presentation.Model.Response
{
    public class MapResponse
    {
        public MapResponse()
        {
            AsphaltList = new List<AsphaltResponse>();
            WaterList = new List<WaterResponse>();
            CollectList = new List<CollectResponse>();
            TrashList = new List<TrashResponse>();
            LightList = new List<LightResponse>();
            SewerList = new List<SewerResponse>();
            Regions = new List<Issues>();
        }
        public List<AsphaltResponse> AsphaltList { get; set; }
        public List<WaterResponse> WaterList { get; set; }
        public List<CollectResponse> CollectList { get; set; }
        public List<TrashResponse> TrashList { get; set; }
        public List<LightResponse> LightList { get; set; }
        public List<SewerResponse> SewerList { get; set; }

        public List<Issues> Regions { get; set; }
    }

    public class Issues {
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}