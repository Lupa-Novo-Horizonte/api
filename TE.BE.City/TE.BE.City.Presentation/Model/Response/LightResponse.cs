using System;

namespace TE.BE.City.Presentation.Model.Response
{
    public class LightResponse : BaseResponse
    {
        // Possui poste?
        public bool HasLight { get; set; }
        // As luzes estão funcionanod?
        public bool IsItWorking { get; set; }
        // Há fios elétricos soltos nos postes?
        public bool HasLosesCable { get; set; }
    }
}
