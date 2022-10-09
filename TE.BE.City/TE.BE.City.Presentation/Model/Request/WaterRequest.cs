using System;
using System.ComponentModel.DataAnnotations;

namespace TE.BE.City.Presentation.Model.Request
{
    /// <summary>
    /// Model responsable for itens on the user interface. It represent the user interface. Not related to the database tables or domain layer.
    /// </summary>
    public class WaterRequest
    {
        public int Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public bool HomeWithWater { get; set; }
        public int WaterMissedInAWeek { get; set; }
        public bool HasWell { get; set; }
        public bool HasSanitationProject { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
    }   
}
