using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Presentation.Model.Response
{
    public class AsphaltResponse : BaseResponse
    {
        public string Title { get { return "Calçadas e Asfalto"; } }
        public TypeIssue Type { get { return TypeIssue.Asphalt; } }
        public bool IsPaved { get; set; }
        //A via possui buracos ou crateras?
        public bool HasHoles { get; set; }
        // Há calçadas pavimentadas?
        public bool HasPavedSidewalks { get; set; }
        // Armazena pontos de um reta no mapa
        public string Path { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem { get; set; }
    }
}
