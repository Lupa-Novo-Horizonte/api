using System;

namespace TE.BE.City.Presentation.Model.Response
{
    public class TrashResponse : BaseResponse
    {
        // Existe limpeza da prefeitura na sua rua?
        public bool HasRoadcleanUp { get; set; }
        // Se sim, qual a frequencia?
        public int HowManyTimes { get; set; }
        // Existe lixo acumulado na rua?
        public bool HasAccumulatedTrash { get; set; }
        // A prefeitura faz a limpeza/capinagem dos matos?
        public string HasLandWeeding { get; set; }
    }
}
