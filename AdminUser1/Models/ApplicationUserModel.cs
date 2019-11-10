using AdminUser1.Classes;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminUser1.Models
{
    public class ApplicationUserModel : ApplicationUser
    {
        public List<IdentityRole> Roles { get; set; }
        public List<IdentityRole> AvailableRoles { get; set; }
        public ApplicationUserModel()
        {
            Roles = new List<IdentityRole>();
            AvailableRoles = new List<IdentityRole>();
        }

        public ApplicationUserModel(ApplicationUser IU) : this()
        {
            NickName = IU.NickName;
            FirstName = IU.FirstName;
            Id = IU.Id;
            UserName = IU.UserName;
            Email = IU.Email;
        }
    }

}
