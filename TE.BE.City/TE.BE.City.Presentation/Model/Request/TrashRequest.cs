namespace TE.BE.City.Presentation.Model.Request
{
    public class TrashRequest : BaseModel
    {
        // Existe limpeza da prefeitura na sua rua?
        public bool HasRoadCleanUp { get; set; }
        // Se sim, qual a frequencia?
        public int HowManyTimes { get; set; }
        // Existe lixo acumulado na rua?
        public bool HasAccumulatedTrash { get; set; }
        // A prefeitura faz a limpeza/capinagem dos matos?
        public bool HasLandWeeding { get; set; }
    }
}
