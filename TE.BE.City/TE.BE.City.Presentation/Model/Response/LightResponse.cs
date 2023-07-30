using System;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Presentation.Model.Response
{
    public class LightResponse : BaseResponse
    {
        public string Title { get { return "Iluminação Pública"; } }
        public TypeIssue Type { get { return TypeIssue.Light; } }
        // Possui poste?
        public bool HasLight { get; set; }
        // As luzes estão funcionanod?
        public bool IsItWorking { get; set; }
        // Há fios elétricos soltos nos postes?
        public bool HasLosesCable { get; set; }
        // Armazena ontas de um reta no mapa
        public string Path { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem { get; set; }
    }
}
