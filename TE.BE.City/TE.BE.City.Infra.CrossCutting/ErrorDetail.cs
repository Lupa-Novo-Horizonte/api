using System;
using System.Collections.Generic;
using System.Text;

namespace TE.BE.City.Infra.CrossCutting
{
    public class ErrorDetail
    {
        public int Code { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
