using IdentityNET10.Models.Entities;
using IdentityNET10.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityNET10.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            AppUser user = new()
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                Name = model.Name.Trim(),
                CountryCode = model.CountryCode,
                Country = model.Country.Trim(),
                City = model.City.Trim(),
                Address = model.Address.Trim(),
                Birthdate = model.Birthdate,
                IsActive = true
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password.Trim());
            if (result.Succeeded)
            {
                //await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email.Trim(), model.Password.Trim(), model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Credenciales de acceso incorrectas.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var user = _userManager.GetUserAsync(User).Result;
            return View(user);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}