using System.ComponentModel.DataAnnotations;

namespace TE.BE.City.Presentation.Model.Request
{
    public class UserRequest
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public int RoleId { get; set; }
    }
}
