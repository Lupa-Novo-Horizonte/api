using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TE.BE.City.Presentation.Model.Response
{
    public class CollectResponse : BaseResponse
    {
        // Existe coleta de lixo na sua casa?
        public bool HasCollect { get; set; }
        // Qual a frequencia semanal?
        public int HowManyTimes { get; set; }
    }
}
