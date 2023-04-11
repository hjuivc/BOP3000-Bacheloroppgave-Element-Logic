using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using static WebStoreElementLogic.Pages.UserPage;

namespace WebStoreElementLogic.Entities
{
    public class User : IdentityUser<int>
    {
        [Required]
        public int userId { get; set; }

        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers.")]
        public string userName { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        public string password { get; set; }

        public bool admin { get; set; }

        public bool IsDeleting { get; set; }

        public bool IsEditing { get; set; }


        public User()
        {
            userId = 0;
            userName = "";
            password = "";
            admin = false;
        }

        public User(int id, string username, string hashedPassword, bool isAdmin)
        {
            userId = id;
            userName = username;
            password = hashedPassword;
            admin = Convert.ToBoolean(isAdmin);
        }
    }
}
