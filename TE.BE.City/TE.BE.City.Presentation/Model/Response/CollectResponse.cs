using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Presentation.Model.Response
{
    public class CollectResponse : BaseResponse
    {
        public string Title { get { return "Coleta de Lixo"; } }
        public TypeIssue Type { get { return TypeIssue.Collect; } }
        // Existe coleta de lixo na sua casa?
        public bool HasCollect { get; set; }
        // Qual a frequencia semanal?
        public int HowManyTimes { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem { get; set; }
    }
}
