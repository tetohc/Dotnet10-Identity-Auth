using System.ComponentModel.DataAnnotations;

namespace IdentityNET10.Models.ViewModels
{
    public class TwoFactorAuthViewModel
    {
        [Display(Name = "Clave")]
        public string KeySecret { get; set; } = null!;
        
        [Display(Name = "Código QR")]
        public string CodeQR { get; set; } = null!;

        [Display(Name = "Código de verificación")]
        public string VerificationCode { get; set; } = null!;
    }
}
