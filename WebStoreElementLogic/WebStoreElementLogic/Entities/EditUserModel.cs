using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace WebStoreElementLogic.Entities
{
    public class EditUserModel
    {
        public int userId { get; set; }

        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers.")]
        public string userName { get; set; }

        public string password { get; set; }

        public bool admin { get; set; }
    }

}
