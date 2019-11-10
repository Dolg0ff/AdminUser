using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using AdminUser1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using AdminUser1.Classes;

namespace TGRSite.Classes
{
    public class Auth : TypeFilterAttribute
    {
        public Auth(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }

        public Auth(params string[] roles) : base(typeof(RoleRequiredFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class RoleRequiredFilter : IAuthorizationFilter
    {
        readonly string[] _roles;
        ApplicationDbContext database;

        public RoleRequiredFilter(string[] roles, ApplicationDbContext db)
        {
            _roles = roles;
            database = db;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                ServiceCollection services = new ServiceCollection();
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(database.Database.GetDbConnection().ConnectionString));
                services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
                services.Configure<IdentityOptions>(o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequiredLength = 5;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireUppercase = false;
                }
                );

                ServiceProvider provider = services.BuildServiceProvider();

                ApplicationDbContext adb = provider.GetRequiredService<ApplicationDbContext>();

                UserManager<ApplicationUser> um = provider.GetRequiredService<UserManager<ApplicationUser>>();
                ApplicationUser u = um.GetUserAsync(context.HttpContext.User).Result;

                if (!adb.IsInRole(u, _roles))
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
