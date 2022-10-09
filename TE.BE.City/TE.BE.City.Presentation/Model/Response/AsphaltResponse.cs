namespace TE.BE.City.Presentation.Model.Response
{
    public class AsphaltResponse : BaseResponse
    {
        public bool IsPaved { get; set; }
        //A via possui buracos ou crateras?
        public bool HasHoles { get; set; }
        // Há calçadas pavimentadas?
        public bool HasPavedSidewalks { get; set; }
        // Armazena pontos de um reta no mapa
        public string Path { get; set; }
    }
}
