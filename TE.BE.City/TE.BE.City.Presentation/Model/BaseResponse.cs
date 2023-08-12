using System;
using TE.BE.City.Infra.CrossCutting;

namespace TE.BE.City.Presentation.Model
{
    public class BaseResponse
    {
        public int Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EndDate { get; set; }
        public ErrorDetail Error { get; set; }
        public int StatusId { get; set; }
    }
}
