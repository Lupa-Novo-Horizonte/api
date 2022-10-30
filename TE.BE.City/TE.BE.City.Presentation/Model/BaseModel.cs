using System;

namespace TE.BE.City.Presentation.Model
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndDate { get; set; }

        public string? Longitude { get; set; }
        public string? Latitude { get; set; }

        public int StatusId { get; set; }
        public int UserId { get; set; }
    }
}
