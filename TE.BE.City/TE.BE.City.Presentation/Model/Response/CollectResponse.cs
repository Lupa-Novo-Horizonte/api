using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TE.BE.City.Presentation.Model.Response
{
    public class CollectResponse : BaseResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Closed { get; set; }
    }
}
