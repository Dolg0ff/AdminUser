using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminUser1.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        public List<IdentityRole> Roles { get; set; }
        public List<IdentityRole> AvailableRoles { get; set; }
        public ApplicationUserModel()
        {
            Roles = new List<IdentityRole>();
            AvailableRoles = new List<IdentityRole>();
        }

        public ApplicationUserModel(IdentityUser IU):this()
        {
            Id = IU.Id;
            UserName = IU.UserName;
            Email = IU.Email;
        }
    }
    
}
