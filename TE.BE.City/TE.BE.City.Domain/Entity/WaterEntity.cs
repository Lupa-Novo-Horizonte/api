using System;

namespace TE.BE.City.Domain.Entity
{
    public class WaterEntity : BaseEntity
    {
        public WaterEntity()
        {
            Status = new StatusEntity();
            User = new UserEntity();
        }

        // Possui água encanada em casa?
        public bool HomeWithWater { get; set; }
        // Quantos dias faltam água na semana?
        public int WaterMissedInAWeek { get; set; }
        // Possui posso de água?
        public bool HasWell { get; set; }
        // A prefeitura está excutando algum projetode saneamento?
        public bool HasSanitationProject { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem => !HomeWithWater;
    }
}
