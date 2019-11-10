using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminUser1.Classes
{
    public class ApplicationUser: IdentityUser
    {
        public string NickName { get; set; }
        public string FirstName { get; set; }
    }
}
