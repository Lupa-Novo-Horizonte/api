namespace TE.BE.City.Domain.Entity
{
    public class SewerEntity : BaseEntity
    {
        public SewerEntity()
        {
            Status = new StatusEntity();
            User = new UserEntity();
        }

        // sua casa possui coleta de esgoto?
        public bool HasHomeSewer { get; set; }
        // Sua casa possui fossa?
        public bool HasHomeCesspool { get; set; }
        // A profeitura estáexecutando algum projeto de saneamento
        public bool HasSanitationProject { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem => !HasHomeSewer;
    }
}
