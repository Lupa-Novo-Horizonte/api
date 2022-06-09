using System.Collections.Generic;

namespace TE.BE.City.Presentation.Model.Response
{
    public class AsphaltSearchResponse
    {
        public AsphaltSearchResponse()
        {
            AsphaltList = new List<AsphaltResponse>();
        }

        public int Total { get; set; }
        public int Page { get; set; }
        public List<AsphaltResponse> AsphaltList { get; set; }
    }
}
