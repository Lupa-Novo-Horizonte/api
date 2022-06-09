using System;
using System.Collections.Generic;

namespace TE.BE.City.Presentation.Model.Response
{
    public class TrashSearchResponse : BaseResponse
    {
        public TrashSearchResponse()
        {
            TrashList = new List<TrashResponse>();
        }

        public int Total { get; set; }
        public int Page { get; set; }
        public List<TrashResponse> TrashList { get; set; }
    }
}
