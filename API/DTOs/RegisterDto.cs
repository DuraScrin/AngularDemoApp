using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        //[RegularExpression("^[A-Za-z]")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}