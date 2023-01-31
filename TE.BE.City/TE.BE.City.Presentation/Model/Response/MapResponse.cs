using System.Collections.Generic;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Presentation.Model.Response
{
    public class MapResponse
    {
        public MapResponse()
        {
            Regions = new List<Issues>();
        }
        
        public List<Issues> Regions { get; set; }
    }

    public class Issues {
        public float? Longitude { get; set; }
        public float? Latitude { get; set; }
        public string? Path { get; set; }
        public string Title { get; set; }
        public TypeIssue Type { get; set; }
        public string Description { get; set; }
    }
}