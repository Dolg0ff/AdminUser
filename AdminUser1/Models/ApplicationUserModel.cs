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
        public ApplicationUserModel()
        {
            Roles = new List<IdentityRole>();
        }
    }
    
}
