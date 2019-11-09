using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminUser1.Data;
using AdminUser1.Models;
using AdminUser1.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminUser1.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _um;
        private readonly RoleManager<IdentityRole> _rm;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> um, RoleManager<IdentityRole> rm)
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
            return View(m);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { Email = model.Email, UserName = model.Email };
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
            IdentityUser user = await _um.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _um.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityUser user = await _um.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _um.FindByIdAsync(model.Id);
                if (user != null)
                {
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