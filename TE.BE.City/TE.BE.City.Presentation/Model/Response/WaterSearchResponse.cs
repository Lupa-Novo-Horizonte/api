using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TE.BE.City.Presentation.Model.Response
{
    /// <summary>
    /// Model responsable for itens on the user interface. It represent the user interface. Not related to the database tables or domain layer.
    /// </summary>
    public class WaterSearchResponse
    {
        public WaterSearchResponse()
        {
            WaterList = new List<WaterResponse>();
        }
        public int Total { get; set; }
        public int Page { get; set; }
        public List<WaterResponse> WaterList { get; set; }
    }   
}
