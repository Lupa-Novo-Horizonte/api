using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TE.BE.City.Presentation.Model.Response
{
    /// <summary>
    /// Model responsable for itens on the user interface. It represent the user interface. Not related to the database tables or domain layer.
    /// </summary>
    public class ReportResponseModel : BaseResponse
    {
        public ReportResponseModel()
        {
            NumberOcorrencyXday = new Dictionary<string, int>();
            NumberOcorrencyXstatusXday = new Dictionary<string, Dictionary<string, int>>();
            NumberOcorrencyXtype = new Dictionary<string, int>();
            LastIncomes = new List<WaterResponse>();
            LastUpdates = new List<WaterResponse>();
        }
        // total of ocorrency peer type. Ex. Sawage=20, Lights=30
        public Dictionary<string, int> NumberOcorrencyXtype { get; set; }

        // total of ocorrency peer day. Ex. 01/01/2021=20, 01/02/2021=30
        public Dictionary<string, int> NumberOcorrencyXday{ get; set; }

        // total of ocorrency peer type. Ex. 01/02/2021|pendent=30,open=10,inExection=5
        public Dictionary<string, Dictionary<string, int>> NumberOcorrencyXstatusXday { get; set; }

        public List<WaterResponse> LastIncomes { get; set; }

        public List<WaterResponse> LastUpdates { get; set; }
    }   
}
