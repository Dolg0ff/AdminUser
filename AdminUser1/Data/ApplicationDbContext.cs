using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AdminUser1.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using AdminUser1.Classes;

namespace AdminUser1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public bool IsInRole(IdentityUser user, String[] roles)
        {
            bool result = roles.Any(x =>
            {
                IQueryable<IdentityUserRole<String>> foundroles = this.UserRoles.Where(xx => xx.UserId == user.Id);
                List<RoleUserResult> temp =
                foundroles.Join(
                    this.Roles,
                    y => y.RoleId,
                    z => z.Id,
                    (r, ru) =>
                    new RoleUserResult
                    {
                        RoleId = r.RoleId,
                        RoleName = ru.Name,
                        UserEmail = user.Email,
                        UserId = user.Id
                    }).Where(zz => zz.RoleName == x).ToList();

                if (temp.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
            return result;
        }

        public async Task<Dictionary<string, ApplicationUserModel>> GetUsersAndRoles(UserManager<ApplicationUser> _um, RoleManager<IdentityRole> _rm)
        {
            Dictionary<string, ApplicationUser> uls = _um.Users.ToDictionary(x => x.Id);
            Dictionary<String, ApplicationUserModel> nusers = new Dictionary<string, ApplicationUserModel>();
            foreach (KeyValuePair<string, ApplicationUser> kuser in uls)
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

        public async Task<ApplicationUserModel> GetUserWithRoles(string id, UserManager<ApplicationUser> _um)
         {
            ApplicationUserModel user = new ApplicationUserModel(this.Users.Where(c => c.Id == id).Single());
            user.Roles = await GetUserRoles(_um, user);

            return user;
        }

        public async Task<List<IdentityRole>> GetUserRoles(UserManager<ApplicationUser> _um, ApplicationUserModel user)
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
    public class RoleUserResult
    {
        public String UserId { get; set; }
        public String RoleId { get; set; }
        public String RoleName { get; set; }
        public String UserEmail { get; set; }
    }
}
