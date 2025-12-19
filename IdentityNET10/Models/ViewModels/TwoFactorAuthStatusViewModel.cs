using System.ComponentModel.DataAnnotations;

namespace IdentityNET10.Models.ViewModels
{
    public class TwoFactorAuthStatusViewModel
    {
        [Display(Name = "Autenticación en dos pasos activada")]
        public bool IsEnabled { get; set; }

        [Display(Name = "Tiene clave de autenticador configurada")]
        public bool HasAuthenticatorKey { get; set; }

        [Display(Name = "Recordar este navegador")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Códigos de recuperación")]
        public string[] RecoveryCodes { get; set; } = Array.Empty<string>();
    }
}