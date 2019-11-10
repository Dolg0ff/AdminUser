using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminUser1.Classes;
using AdminUser1.Data;
using AdminUser1.Models;
using AdminUser1.Views.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TGRSite.Classes;

namespace AdminUser1.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Auth(roles: "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _um;
        private readonly RoleManager<IdentityRole> _rm;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
        {
            _context = context;
            _um = um;
            _rm = rm;
        }

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

        public IActionResult Create() => PartialView();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.Email, NickName = model.NickName, FirstName = model.FirstName };
                var result = await _um.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await _um.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _um.DeleteAsync(user);
            }
            return PartialView();
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await _um.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, NickName = user.NickName, FirstName = user.FirstName };
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _um.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _um.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }
    }
}