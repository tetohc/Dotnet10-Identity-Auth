using System.ComponentModel.DataAnnotations;

namespace IdentityNET10.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "La confirmación de la contraseña es requerida.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
