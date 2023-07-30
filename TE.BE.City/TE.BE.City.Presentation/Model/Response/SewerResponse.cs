using System;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Presentation.Model.Response
{
    public class SewerResponse : BaseResponse
    {
        public string Title { get { return "Tratamento de Esgoto"; } }
        public TypeIssue Type { get { return TypeIssue.Sewer; } }
        // sua casa possui coleta de esgoto?
        public bool HasHomeSewer { get; set; }
        // Sua casa possui fossa?
        public bool HasHomeCesspool { get; set; }
        //A prefeitura limpa os esgotos?
        public bool HasSanitationProject { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem { get; set; }
    }
}
