﻿using System.Collections.Generic;

namespace TE.BE.City.Presentation.Model.Response
{
    public class ChartResponse
    {
        public List<ChartObject> ChartQuantity = new List<ChartObject>();
        public string ChartQuantitySerialized { get; set; }

        public List<ChartProportionObject> ChartProportion = new List<ChartProportionObject>();
        public string ChartProportionSerialized { get; set; }
    }

    public class ChartObject {
        public string label { get; set; }
        public double y { get; set; }
        public string indexLabel { get { return y.ToString(); } }
    }

    public class ChartProportionObject
    {
        public string label { get; set; }
        public double y { get; set; }
        public string indexLabel { get { return (y * 100).ToString() + "%"; } }
    }
}