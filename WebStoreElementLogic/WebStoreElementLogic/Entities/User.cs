using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace WebStoreElementLogic.Entities
{
    public class User : IdentityUser<int>
    {
        [Required]
        public override string UserName { get; set; }
        [Required]
        public override string PasswordHash { get; set; }
        public User(int id, string username, string password)
        {
            Id = id;
            UserName = username;
            PasswordHash = password;
        }

        public User()
        {
            Id = 0;
            UserName = "";
            PasswordHash = "";
        }
    }
}

