using System;

namespace TE.BE.City.Presentation.Model.Response
{
    public class UserResponse : BaseResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string RoleId { get; set; }
        public bool Active { get; set; }
        public bool Block { get; set; }
        public string Token { get; set; }
    }
}
