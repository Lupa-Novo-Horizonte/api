using TE.BE.City.Infra.CrossCutting;

namespace TE.BE.City.Presentation.Model.ViewModel
{
    public class SurveyViewModel
    {
        public string Question01 { get; set; }
        public string Question02 { get; set; }
        public string Question03 { get; set; }
        public string Question04 { get; set; }
        public string Question05 { get; set; }
        public string Question06 { get; set; }
        public string Question07 { get; set; }
        public string Question08 { get; set; }
        public string Question09 { get; set; }
        public string Question10 { get; set; }
        public string Question11 { get; set; }
        public bool ShowConfirmation { get; set; }
        public ErrorDetail Error { get; set; }
    }
}
