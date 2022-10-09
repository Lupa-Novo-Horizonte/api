using System;

namespace TE.BE.City.Presentation.Model.Response
{
    public class SewerResponse : BaseResponse
    {
        // sua casa possui coleta de esgoto?
        public bool HasHomeSewer { get; set; }
        // Sua casa possui fossa?
        public bool HasHomeCesspool { get; set; }
        //A prefeitura limpa os esgotos?
        public bool HasSanitationProject { get; set; }
    }
}
