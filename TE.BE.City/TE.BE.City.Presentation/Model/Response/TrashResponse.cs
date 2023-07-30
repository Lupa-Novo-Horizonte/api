using System;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Presentation.Model.Response
{
    public class TrashResponse : BaseResponse
    {
        public string Title { get { return "Limpeza Urbana"; } }
        public TypeIssue Type { get { return TypeIssue.Trash; } }
        // Existe limpeza da prefeitura na sua rua?
        public bool HasRoadcleanUp { get; set; }
        // Se sim, qual a frequencia?
        public int HowManyTimes { get; set; }
        // Existe lixo acumulado na rua?
        public bool HasAccumulatedTrash { get; set; }
        // A prefeitura faz a limpeza/capinagem dos matos?
        public string HasLandWeeding { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem { get; set; }
    }
}
