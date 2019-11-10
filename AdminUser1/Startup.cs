using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using AdminUser1.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using AdminUser1.Classes;

namespace AdminUser1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {

                //options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); 
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvc();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            try
            {
                DBInit.CreateDefaultRolesAndUser(roleManager, userManager);
            }
            catch (Exception)
            {

            }
        }
    }

    public static class DBInit
    {
        public static void CreateDefaultRolesAndUser(RoleManager<IdentityRole> RoleManager, UserManager<ApplicationUser> UserManager, String email = null, String password = null)
        {
            String email_ = "";
            String pass_ = "";

            if (email != null)
            {
                email_ = email;
            }
            else
            {
                email_ = "Dolg0ff@mail.ru";
            }

            if (password != null)
            {
                pass_ = password;
            }
            else
            {
                pass_ = "Qwerty1_";
            }

            string[] roleNames = { "Admin", "Admin", "Editor", "Customer" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                // creating the roles and seeding them to the database
                var roleExist = RoleManager.RoleExistsAsync(roleName).Result;
                if (!roleExist)
                {
                    roleResult = RoleManager.CreateAsync(new IdentityRole(roleName)).Result;
                }
            }

            ApplicationUser user = UserManager.FindByEmailAsync(email_).Result;
            if (user == null)
            {
                ApplicationUser u = new ApplicationUser();
                u.Email = email_;
                u.UserName = email_;
                u.LockoutEnabled = true;
                IdentityResult r = UserManager.CreateAsync(u, pass_).Result;
            }

            ApplicationUser nu = UserManager.FindByEmailAsync(email_).Result;

            if (nu != null)
            {
                IdentityResult rr = UserManager.AddToRoleAsync(nu, "Admin").Result;
            }
        }
    }
}
