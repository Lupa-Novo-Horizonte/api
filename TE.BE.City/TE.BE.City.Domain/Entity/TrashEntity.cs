using System;
using System.Collections.Generic;
using System.Text;

namespace TE.BE.City.Domain.Entity
{
    public class TrashEntity : BaseEntity
    {
        public TrashEntity()
        {
            Status = new StatusEntity();
            User = new UserEntity();
        }
        // Existe limpeza da prefeitura na sua rua?
        public bool HasRoadcleanUp { get; set; }
        // Se sim, qual a frequencia?
        public int HowManyTimes { get; set; }
        // Existe lixo acumulado na rua?
        public bool HasAccumulatedTrash { get; set; }
    }
}
