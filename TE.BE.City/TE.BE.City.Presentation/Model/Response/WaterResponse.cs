using System;
using System.ComponentModel.DataAnnotations;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Presentation.Model.Response
{
    /// <summary>
    /// Model responsable for itens on the user interface. It represent the user interface. Not related to the database tables or domain layer.
    /// </summary>
    public class WaterResponse : BaseResponse
    {
        public string Title { get { return "Água Potável"; } }
        public TypeIssue Type { get { return TypeIssue.Water; } }
        // Possui água encanada em casa?
        public bool HomeWithWater { get; set; }
        // Quantos dias faltam água na semana?
        public int WaterMissedInAWeek { get; set; }
        // Possui posso de água?
        public bool HasWell { get; set; }
        // A prefeitura está excutando algum projetode saneamento?
        public bool HasSanitationProject { get; set; }
        // Informa se o item é considerado um poblem ou apenas um mapeamento
        public bool IsProblem { get; set; }
    }   
}
