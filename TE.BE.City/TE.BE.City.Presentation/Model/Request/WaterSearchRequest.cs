using System;
using System.ComponentModel.DataAnnotations;

namespace TE.BE.City.Presentation.Model.Request
{
    /// <summary>
    /// Model responsable for itens on the user interface. It represent the user interface. Not related to the database tables or domain layer.
    /// </summary>
    public class WaterSearchRequest
    {
        public const int LIMIT = 10;

        public int Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int StatusId { get; set; }
        public bool homeWithWater { get; set; }
        public int waterMissedInAWeek { get; set; }
        public bool hasWell { get; set; }
        public int UserId { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }   
}
