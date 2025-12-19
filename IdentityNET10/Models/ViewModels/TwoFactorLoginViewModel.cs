using System.ComponentModel.DataAnnotations;

namespace IdentityNET10.Models.ViewModels
{
    public class TwoFactorLoginViewModel
    {
        [Required(ErrorMessage = "El código es requerido.")]
        [Display(Name = "Código de autenticación")]
        [StringLength(6, ErrorMessage = "El código debe tener 6 dígitos.")]
        public string Code { get; set; } = string.Empty;

        [Display(Name = "Recordar este dispositivo")]
        public bool RememberDevice { get; set; }

        [Display(Name = "Recordar sesión")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; } = string.Empty;
    }
}