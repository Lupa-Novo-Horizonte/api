using System.Collections.Generic;

namespace TE.BE.City.Presentation.Model.Response
{
    public class StatusSearchResponseModel
    {
        public StatusSearchResponseModel()
        {
            Status = new List<StatusResponseModel>();
        }
        public int Total { get; set; }
        public int Page { get; set; }
        public List<StatusResponseModel> Status { get; set; }
    }
}
