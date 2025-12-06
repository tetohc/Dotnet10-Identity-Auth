namespace IdentityNET10.Models.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
    }
}