using System;
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
    }
}
