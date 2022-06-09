using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TE.BE.City.Infra.CrossCutting;

namespace TE.BE.City.Domain.Entity
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndDate { get; set; }

        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public ErrorDetail Error { get; set; }

        public int StatusId { get; set; }
        public StatusEntity Status { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public bool IsSuccess
        {
            get
            {
                if (Error == null)
                    return true;
                else
                    return false;
            }
        }
    }
}
