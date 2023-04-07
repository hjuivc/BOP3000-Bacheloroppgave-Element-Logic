using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace WebStoreElementLogic.Entities
{
    public class EditUserModel
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }

}
