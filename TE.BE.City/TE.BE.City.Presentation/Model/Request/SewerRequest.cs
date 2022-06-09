using System.ComponentModel.DataAnnotations;

namespace TE.BE.City.Presentation.Model.Request
{
    public class SewerRequest : BaseModel
    {
        // sua casa possui coleta de esgoto?
        public bool HasHomeSewer { get; set; }
        // Sua casa possui fossa?
        public bool HasHomeCesspool { get; set; }
        //A prefeitura limpa os esgotos?
        public bool DoesCityHallCleanTheSewer { get; set; }
    }
}
