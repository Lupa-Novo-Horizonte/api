using System;
using System.Collections.Generic;
using System.Text;

namespace TE.BE.City.Domain.Entity
{
    public class CollectEntity : BaseEntity
    {
        public CollectEntity()
        {
            Status = new StatusEntity();
            User = new UserEntity();
        }

        // Existe coleta de lixo na sua casa?
        public bool HasCollect { get; set; }
        // Qual a frequencia semanal?
        public int HowManyTimes { get; set; }
    }
}
