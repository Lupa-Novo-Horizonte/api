﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TE.BE.City.Domain.Entity
{
    public class AsphaltEntity : BaseEntity
    {
        public AsphaltEntity()
        {
            Status = new StatusEntity();
            User = new UserEntity();
        }

        //A via é asfaltada?
        public bool IsPaved { get; set; }
        //A via possui buracos ou crateras?
        public bool HasHoles { get; set; }
        // Há calçadas pavimentadas?
        public bool HasPavedSidewalks { get; set; }
        // Armazena pontos de um reta no mapa
        public string Path { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem => !IsPaved || !HasPavedSidewalks || HasHoles;
    }
}
