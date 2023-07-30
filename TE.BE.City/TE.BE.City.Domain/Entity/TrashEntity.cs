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
        public bool HasRoadCleanUp { get; set; }
        // Se sim, qual a frequencia?
        public int HowManyTimes { get; set; }
        // Existe lixo acumulado na rua?
        public bool HasAccumulatedTrash { get; set; }
        // A prefeitura faz a limpeza/capinagem dos matos?
        public bool HasLandWeeding { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem => !HasRoadCleanUp || HasAccumulatedTrash || HasLandWeeding;
    }
}
