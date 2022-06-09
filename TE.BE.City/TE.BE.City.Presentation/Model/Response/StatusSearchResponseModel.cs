using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TE.BE.City.Presentation.Model.Response
{
    public class StatusSearchResponseModel : BaseResponse
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
