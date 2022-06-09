using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TE.BE.City.Presentation.Model.Response
{
    public class AuthenticateResponse : BaseResponse
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
