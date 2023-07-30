using System;
using System.Collections.Generic;
using System.Text;

namespace TE.BE.City.Domain.Entity
{
    public class LightEntity : BaseEntity
    {
        public LightEntity()
        {
            Status = new StatusEntity();
            User = new UserEntity();
        }

        // Possui poste?
        public bool HasLight { get; set; }
        // As luzes estão funcionanod?
        public bool IsItWorking { get; set; }
        // Há fios elétricos soltos nos postes?
        public bool HasLosesCable { get; set; }
        // Armazena ontas de um reta no mapa
        public string Path { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem => !HasLight || !IsItWorking ||HasLosesCable;
    }
}
