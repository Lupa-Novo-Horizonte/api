using System.Collections.Generic;

namespace TE.BE.City.Presentation.Model.Response
{
    public class LightSearchResponse
    {
        public LightSearchResponse()
        {
            LightList = new List<LightResponse>();
        }

        public int Total { get; set; }
        public int Page { get; set; }
        public List<LightResponse> LightList { get; set; }
    }
}
