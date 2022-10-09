using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TE.BE.City.Presentation.Model.Response
{
    public class CollectSearchResponse
    {
        public CollectSearchResponse()
        {
            CollectList = new List<CollectResponse>();
        }
        public int Total { get; set; }
        public int Page { get; set; }
        public List<CollectResponse> CollectList { get; set; }
    }
}
