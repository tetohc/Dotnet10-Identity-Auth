using IdentityNET10.Extensions;
using IdentityNET10.Models.Entities;
using IdentityNET10.Models.ViewModels;
using IdentityNET10.Templates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;

namespace IdentityNET10.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailSender emailSender,
           ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailSender;
            _logger = logger;
        }

        #region Registro de usuarios

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
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var confirmationUrl = Url.Action("ConfirmEmail", "Account", new { id = user.Id, token = tokenEncoded }, Request.Scheme);
                var html = EmailTemplates.ConfirmEmailTemplate(confirmationUrl!, user.Name);
                await _emailService.SendEmailAsync(user.Email, "Confirmar correo electrónico", html);

                return RedirectToAction(nameof(RegisterConfirmation));
            }
            return View(model);
        }

        #endregion Registro de usuarios

        #region Inicio de sesión interno

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

            var user = await _userManager.FindByEmailAsync(model.Email.Trim());
            if (user is null)
            {
                _logger.LogWarning($"Intento de inicio de sesión fallido para el usuario {model.Email}.");
                return View(model);
            }

            if (!user!.EmailConfirmed)
                return RedirectToAction(nameof(EmailNotConfirmed));

            var result = await _signInManager.PasswordSignInAsync(model.Email.Trim(), model.Password.Trim(), model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }
            else if (result.RequiresTwoFactor)
                return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, rememberMe = model.RememberMe });

            _logger.LogWarning($"Intento de inicio de sesión fallido para el usuario {model.Email}.");
            return View(model);
        }

        [HttpGet]
        public IActionResult LoginWith2fa(string? returnUrl, bool rememberMe)
        {
            var model = new TwoFactorLoginViewModel
            {
                ReturnUrl = returnUrl ?? Url.Content("~/"),
                RememberMe = rememberMe
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LoginWith2fa(TwoFactorLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var code = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(
                code,
                model.RememberMe,
                model.RememberDevice
            );

            if (result.Succeeded)
                return Redirect(model.ReturnUrl ?? Url.Content("~/"));

            _logger.LogWarning("Código de autenticación de dos factores inválido.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        #endregion Inicio de sesión interno

        #region Inicio de sesión externo

        [HttpPost]
        public async Task<IActionResult> ExternalLogin(string provider, string? returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError is not null)
            {
                _logger.LogWarning($"Error del proveedor externo: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
                return RedirectToAction(nameof(Login));

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
                return LocalRedirect(returnUrl);

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);
            if (email is null)
                return RedirectToAction(nameof(Login));

            var user = new AppUser
            {
                UserName = email,
                Email = email,
                Name = name ?? email,
                EmailConfirmed = true,
                IsActive = true,
                Address = string.Empty,
                City = string.Empty,
                Country = string.Empty,
                CountryCode = 0,
            };
            var identityResult = await _userManager.CreateAsync(user);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                    _logger.LogWarning($"Error: {error.Description}");
                return RedirectToAction(nameof(Login));
            }

            identityResult = await _userManager.AddLoginAsync(user, info);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                    _logger.LogWarning($"Error: {error.Description}");
                return RedirectToAction(nameof(Login));
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            var model = new ExternalLoginConfirmationViewModel
            {
                Email = user.Email,
                ReturnUrl = returnUrl,
                Provider = info.LoginProvider
            };
            return View("ExternalLoginConfirmation", model);
        }

        [HttpGet]
        public IActionResult ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmExternalLogin(string returnUrl)
        {
            return LocalRedirect(returnUrl ?? "~/");
        }

        #endregion Inicio de sesión externo

        #region Confirmar Correo Electrónico

        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(token))
                return RedirectToAction(nameof(ConfirmEmailError));

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return RedirectToAction(nameof(ConfirmEmailError));

            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(token);
            var tokenDecoded = Encoding.UTF8.GetString(tokenDecodedBytes);
            var result = await _userManager.ConfirmEmailAsync(user, tokenDecoded);
            if (!result.Succeeded)
                return RedirectToAction(nameof(ConfirmEmailError));

            return View();
        }

        [HttpGet]
        public IActionResult ConfirmEmailError()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailNotConfirmed()
        {
            return View();
        }

        #endregion Confirmar Correo Electrónico

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

        #region Autenticación en Dos Factores (2FA)

        /// <summary>
        /// Prepara la configuración de la autenticación de dos factores (2FA) para el usuario actual.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SetupTwoFactorAuth()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return RedirectToAction(nameof(Login), "Account");

            var key = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(key))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                key = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var qrCodeUri = key!.ToQrCodeUri(user.Email!);
            var model = new TwoFactorAuthViewModel
            {
                KeySecret = key!.ToReadableFormat(),
                CodeQR = qrCodeUri!.GenerateQrCodeImage(),
            };
            return View(model);
        }

        /// <summary>
        /// Activa la autenticación de dos factores (2FA) para el usuario actual después de verificar el código proporcionado.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EnableTwoFactorAuth(TwoFactorAuthViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return RedirectToAction(nameof(Login), "Account");

            var token = model.VerificationCode.Replace(" ", string.Empty).Replace("-", string.Empty);
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                _userManager.Options.Tokens.AuthenticatorTokenProvider,
                token
            );
            if (!isValid)
            {
                _logger.LogWarning("Código de verificación inválido.");
                return View(nameof(SetupTwoFactorAuth), model);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            TempData["RecoveryCodes"] = recoveryCodes!.ToList();
            return RedirectToAction(nameof(ConfirmTwoFactorAuth), "Account", new { codes = recoveryCodes!.ToList() });
        }

        /// <summary>
        /// Muestra los códigos de recuperación generados después de habilitar la autenticación de dos factores (2FA).
        /// </summary>
        [HttpGet]
        public IActionResult ConfirmTwoFactorAuth(IEnumerable<string> codes)
        {
            ViewBag.RecoveryCodes = codes;
            return View();
        }

        /// <summary>
        /// Muestra el estado de la autenticación de dos factores (2FA) para el usuario actual.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthStatusViewModel()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound();

            var model = await TwoFactorAuthExtensions.BuildTwoFactorStatusAsync(_userManager, _signInManager, user);
            if (TempData["RecoveryCodes"] is string[] codes)
                model.RecoveryCodes = codes;

            return View(model);
        }

        /// <summary>
        /// Desactiva la autenticación de dos factores (2FA) para el usuario actual.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> DisableTwoFactorAuth()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound();

            var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!result.Succeeded)
                _logger.LogWarning("Error al deshabilitar la autenticación de dos factores.");

            await _userManager.ResetAuthenticatorKeyAsync(user);
            return RedirectToAction(nameof(TwoFactorAuthStatusViewModel));
        }

        /// <summary>
        /// Genera nuevos códigos de recuperación para la autenticación de dos factores (2FA) del usuario actual.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return RedirectToAction(nameof(Login), "Account");

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            TempData["RecoveryCodes"] = recoveryCodes!.ToArray();
            return RedirectToAction(nameof(TwoFactorAuthStatusViewModel), "Account");
        }

        #endregion Autenticación en Dos Factores (2FA)
    }
}