namespace TE.BE.City.Presentation.Model.Request
{
    public class CollectRequest : BaseModel
    {
        // Existe coleta de lixo na sua casa?
        public bool HasCollect { get; set; }
        // Qual a frequencia semanal?
        public int HowManyTimes { get; set; }
        // Existe coleta seletiva?
        public bool HasSelectiveCollect { get; set; }
    }
}
