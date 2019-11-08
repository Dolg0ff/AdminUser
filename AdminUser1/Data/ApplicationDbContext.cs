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
                ApplicationUserModel user = new ApplicationUserModel(kuser.Value);               
                user.Roles = await GetUserRoles(_um, user);
                nusers.Add(user.Id, user);
            }
            return nusers;
        }

        public ApplicationUserModel GetUser(string id)
        {
            ApplicationUserModel user = new ApplicationUserModel(this.Users.Where(c => c.Id == id).Single());
            return user;
        }

        public async Task<ApplicationUserModel> GetUserWithRoles(string id, UserManager<IdentityUser> _um)
        {
            ApplicationUserModel user = new ApplicationUserModel(this.Users.Where(c => c.Id == id).Single());
            user.Roles = await GetUserRoles(_um, user);

            return user;
        }

        public async Task<List<IdentityRole>> GetUserRoles(UserManager<IdentityUser> _um, ApplicationUserModel user)
        {
            IList<String> roles = await _um.GetRolesAsync(user);
            List<IdentityRole> uRoles = new List<IdentityRole>();
            roles.ToList().ForEach(role =>
            {
                IdentityRole r = this.Roles.Where(z => z.NormalizedName == role.ToUpper()).Single();
                uRoles.Add(r);
            });
            return uRoles;
        }
        public async Task<int> AddToRole(string userId, string roleId)
        {
            IdentityUserRole<String> irl = new IdentityUserRole<String>();
            irl.RoleId = roleId;
            irl.UserId = userId;
            this.UserRoles.Add(irl);
            return await this.SaveChangesAsync();
        }
        public async Task<int> RemoveFromRole(string userId, string roleId)
        {
            IdentityUserRole<String> irl = this.UserRoles.Where(r => r.UserId == userId && r.RoleId == roleId).Single();
            this.UserRoles.Remove(irl);
            return await this.SaveChangesAsync();
        }
    }
}
