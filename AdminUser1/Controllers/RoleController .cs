using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminUser1.Data;
using AdminUser1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TGRSite.Classes;

namespace AdminUser1.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Auth(roles: "Admin")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _rm;

        public RoleController(ApplicationDbContext context, RoleManager<IdentityRole> rm)
        {
            _context = context;
            _rm = rm;
        }
        // GET: UserModels
        public async Task<IActionResult> Index()
        {
            List<IdentityRole> rls = await _context.Roles.ToListAsync();
            return PartialView(rls);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            IdentityRole role = new IdentityRole();
            role.Name = roleName;
            IdentityResult res = await _rm.CreateAsync(role);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddToRole(string userId, string Role )
        {
            int res = await _context.AddToRole(userId, Role);
            return RedirectToAction("UserEdit", "Admin", new { id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string UserId, string RoleId)
        {
            int res = await _context.RemoveFromRole(UserId, RoleId);
            return RedirectToAction("UserEdit", "Admin", new { id = UserId });
        }
    }
}