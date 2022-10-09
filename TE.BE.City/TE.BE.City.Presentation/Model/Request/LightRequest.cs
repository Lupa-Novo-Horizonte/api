namespace TE.BE.City.Presentation.Model.Request
{
    public class LightRequest : BaseModel
    {
        // Possui poste?
        public bool HasLight { get; set; }
        // As luzes estão funcionanod?
        public bool IsItWorking { get; set; }
        // Há fios elétricos soltos nos postes?
        public bool HasLosesCable { get; set; }
        // Armazena ontas de um reta no mapa
        public string Path { get; set; }
    }
}
