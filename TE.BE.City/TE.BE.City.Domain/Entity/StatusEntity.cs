using System;
using System.Collections.Generic;
using System.Text;

namespace TE.BE.City.Domain.Entity
{
    public class StatusEntity : BaseEntity
    {
        public string Name { get; set; }
        public bool Closed { get; set; }
    }
}
