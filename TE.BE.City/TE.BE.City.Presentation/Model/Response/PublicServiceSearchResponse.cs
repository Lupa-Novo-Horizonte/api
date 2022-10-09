using System.Collections.Generic;

namespace TE.BE.City.Presentation.Model.Response
{
    public class PublicServiceSearchResponse
    {
        public PublicServiceSearchResponse()
        {
            PublicServiceList = new List<PublicServiceResponse>();
        }
        public int Total { get; set; }
        public int Page { get; set; }
        public List<PublicServiceResponse> PublicServiceList { get; set; }
    }
}
