using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminUser1.Classes;
using AdminUser1.Data;
using AdminUser1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TGRSite.Classes;

namespace AdminUser1.Controllers
{
    // [Authorize(Roles ="Admin")]
    [Auth(roles: "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _um;
        private readonly RoleManager<IdentityRole> _rm;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
        {
            _context = context;
            _um = um;
            _rm = rm;
        }
        // GET: UserModels
        public async Task<IActionResult> Index()
        {
            List<IdentityRole> rls = _context.Roles.ToList();
            UserModel m = new UserModel
            {
                Users = await _context.GetUsersAndRoles(_um, _rm),
                Roles = rls
            };
            return PartialView(m);
        }

        [HttpGet]
        public async Task<IActionResult> UserEdit(string id)
        {
            ApplicationUserModel u = await _context.GetUserWithRoles(id, _um);
            List<IdentityRole> rls = _context.Roles.AsNoTracking().ToList();
            u.AvailableRoles = rls;

            return PartialView(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(ApplicationUserModel user)
        {
            return PartialView();
        }


    }
}