using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace WebStoreElementLogic.Entities
{
    public class User : IdentityUser<int>
    {
        [Required]
        public int userId { get; set; }

        [Required]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }

        public User()
        {
            userId = 0;
            userName = "";
            password = "";
        }
        public User(int id, string username, string password)
        {
            userId = id;
            userName = username;
            password = password;
        }

     

    }
}

