using System.ComponentModel.DataAnnotations;

namespace IdentityNET10.Models.ViewModels
{
    public class SignUpViewModel
    {
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

        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(45, ErrorMessage = "El nombre no debe exceder los 45 caracteres.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El código de país es requerido.")]
        [Range(1, 999, ErrorMessage = "Ingrese un código de país válido.")]
        [Display(Name = "Código de país")]
        public int CountryCode { get; set; }

        [Required(ErrorMessage = "El país es requerido.")]
        [Display(Name = "País")]
        public string Country { get; set; } = null!;

        [Required(ErrorMessage = "La ciudad es requerida.")]
        [Display(Name = "Ciudad")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es requerida.")]
        [Display(Name = "Dirección")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de nacimiento es requerida.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateOnly Birthdate { get; set; }

        [Display(Name = "Estado")]
        public bool IsActive { get; set; }
    }
}