using IdentityNET10.Models.Entities;
using IdentityNET10.Models.ViewModels;
using IdentityNET10.Templates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace IdentityNET10.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailSender;
        }

        #region Registro e Inicio de sesión

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

        #endregion Registro e Inicio de sesión

        #region Usuario

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

        #endregion Usuario

        #region Restablecer Contraseña

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = model.Email.Trim();
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetUrl = Url.Action("ResetPassword", "Account", new { email, token = tokenEncoded }, Request.Scheme);

            var html = EmailTemplates.ForgotPasswordTemplate(resetUrl!);
            await _emailService.SendEmailAsync(email, "Restablecer contraseña", html);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var model = new ResetPasswordViewModel { Email = email, Token = token };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = model.Email.Trim();
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return RedirectToAction(nameof(ResetPasswordConfirmation));

            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(model.Token);
            var tokenDecoded = Encoding.UTF8.GetString(tokenDecodedBytes);
            var result = await _userManager.ResetPasswordAsync(user, tokenDecoded, model.Password.Trim());

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion Restablecer Contraseña
    }
}