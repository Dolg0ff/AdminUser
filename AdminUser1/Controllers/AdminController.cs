using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminUser1.Data;
using AdminUser1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminUser1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _um;
        private readonly RoleManager<IdentityRole> _rm;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> um, RoleManager<IdentityRole> rm)
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
            return View(m);
        }
    }
}