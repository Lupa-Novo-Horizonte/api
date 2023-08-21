using System;
using System.Collections.Generic;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Presentation.Model.Response
{
    public class ChartResponse
    {
        public List<ChartObject> ChartQuantity = new List<ChartObject>();
        public string ChartQuantitySerialized { get; set; }

        public List<ChartProportionObject> ChartProportion = new List<ChartProportionObject>();
        public string ChartProportionSerialized { get; set; }

        public Dictionary <TypeIssue, ChartTable> ChartTable { get; set; }
    }

    public class ChartObject {
        public string label { get; set; }
        public double y { get; set; } = 0;
        public string indexLabel { get { return y.ToString(); } }
    }

    public class ChartProportionObject
    {
        public string label { get; set; }
        public double y { get; set; } = 0;
        public string indexLabel { get { return (Math.Round(y * 100, 2)).ToString() + "%"; } }
    }

    public class ChartTable
    {
        public int ProblemCount { get; set; }
        public int NoProblemCount { get; set; }
        public int Total
        {
            get
            {
                return ProblemCount + NoProblemCount;
            }
        }
    }
}
