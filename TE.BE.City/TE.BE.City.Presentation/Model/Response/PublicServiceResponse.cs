using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Presentation.Model.Response
{
    public class PublicServiceResponse : BaseResponse
    {
        public string Title { get { return "Serviços e Referências"; } }
        public TypeIssue Type { get { return TypeIssue.PublicService; } }
        public int Service { get; set; }
    }
}
