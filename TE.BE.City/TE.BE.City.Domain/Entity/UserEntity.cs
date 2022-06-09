using System;

namespace TE.BE.City.Domain.Entity
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool Block { get; set; }
        public string Token { get; set; }
        public int RoleId { get; set; }
    }
}
