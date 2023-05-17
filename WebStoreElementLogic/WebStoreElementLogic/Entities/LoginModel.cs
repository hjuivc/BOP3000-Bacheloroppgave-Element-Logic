using System.ComponentModel.DataAnnotations;

namespace WebStoreElementLogic.Entities
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter your username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        [Required]
        public bool IsAdmin { get; set; }
    }
}
