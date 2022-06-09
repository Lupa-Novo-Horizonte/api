using System;
using System.Collections.Generic;
using System.Text;
using TE.BE.City.Infra.CrossCutting;

namespace TE.BE.City.Domain.Entity
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
