using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AdminUser1.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace AdminUser1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public async Task<Dictionary<string, ApplicationUserModel>> GetUsersAndRoles(UserManager<IdentityUser> _um, RoleManager<IdentityRole> _rm)
        {
            Dictionary<string, IdentityUser> uls = _um.Users.ToDictionary(x => x.Id);
            Dictionary<String, ApplicationUserModel> nusers = new Dictionary<string, ApplicationUserModel>();
            foreach (KeyValuePair<string, IdentityUser> kuser in uls)
            {
                ApplicationUserModel user = new ApplicationUserModel();
                user.Id = kuser.Value.Id;
                user.UserName = kuser.Value.UserName;
                user.Email = kuser.Value.Email;
                IList<String> roles = await _um.GetRolesAsync(user);

                roles.ToList().ForEach(role =>
                {
                    IdentityRole r = this.Roles.Where(z => z.NormalizedName == role.ToUpper()).Single();
                    user.Roles.Add(r);
                });
                nusers.Add(user.Id, user);
            }
            return nusers;
        }
    }
}
