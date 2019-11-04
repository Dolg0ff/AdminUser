using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminUser1.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public Dictionary<string, ApplicationUserModel> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}
