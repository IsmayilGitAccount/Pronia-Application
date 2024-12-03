using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProniaApplication.Models;
using ProniaApplication.ViewModels;

namespace ProniaApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if (!ModelState.IsValid) 
            {
            return View();
            }

            AppUser user = new AppUser()
            {
                Name = userVM.Name,
                Surname = userVM.Surname,
                Email = userVM.Email,
                UserName = userVM.UserName,
            };

            IdentityResult result = await _userManager.CreateAsync(user, userVM.Password);

            if (!result.Succeeded) 
            {
                foreach (IdentityError error in result.Errors) 
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await _signinManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
