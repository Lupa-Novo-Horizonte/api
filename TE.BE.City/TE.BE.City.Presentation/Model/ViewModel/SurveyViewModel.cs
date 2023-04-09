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
        public bool ShowConfirmation { get; set; }
        public ErrorDetail Error { get; set; }
    }
}
