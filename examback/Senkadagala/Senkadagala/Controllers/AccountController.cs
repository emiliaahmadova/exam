using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Senkadagala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senkadagala.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }

        public async Task<IActionResult> Register()
        {
            IdentityUser newUser = new IdentityUser()
            {
                Email = "admin@mail.ru",
                UserName = "admin"
            };

            IdentityResult result = await userManager.CreateAsync(newUser, "SalamSalam123");

            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return Content("Not okay");
            }

            return Content("Okay");
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Your email or password is wrong");
                return View(viewModel);
            }

            IdentityUser loggingUser = await userManager.FindByEmailAsync(viewModel.Email);

            if (loggingUser == null)
            {
                return View(viewModel);
            }

           Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(loggingUser, viewModel.Password, viewModel.StayLoggedIn, false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "You are locked out");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is wrong");
                }
            }
            return RedirectToAction("Index", "Home", new { area = "admin" });
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
