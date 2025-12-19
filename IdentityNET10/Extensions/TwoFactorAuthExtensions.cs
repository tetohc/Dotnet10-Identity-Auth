using IdentityNET10.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using QRCoder;

namespace IdentityNET10.Extensions
{
    public static class TwoFactorAuthExtensions
    {
        /// <summary>
        /// Da formato legible a una clave dividiéndola en bloques de 4 caracteres separados por " ".
        /// </summary>
        public static string ToReadableFormat(this string key)
            => string.Join(" ", Enumerable.Range(0, key.Length / 4)
                .Select(i => key.Substring(i * 4, 4)));

        /// <summary>
        /// Genera una URI para el código QR utilizado en la autenticación de dos factores TOTP.
        /// </summary>
        public static string ToQrCodeUri(this string key, string email, string issuer = "IdentityNET10")
        {
            string normalizedKey = key.Replace(" ", string.Empty).ToUpperInvariant();
            return $"otpauth://totp/{issuer}:{email}?secret={normalizedKey}&issuer={issuer}&digits=6";
        }

        /// <summary>
        /// Genera una imagen de código QR en formato base64 a partir de una URI de código QR.
        /// </summary>
        public static string GenerateQrCodeImage(this string qrCodeUri)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);
            return $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}";
        }

        /// <summary>
        /// Construye un modelo de vista que representa el estado de la autenticación de dos factores.
        /// </summary>
        public static async Task<TwoFactorAuthStatusViewModel> BuildTwoFactorStatusAsync<TUser>(
            UserManager<TUser> userManager,
            SignInManager<TUser> signInManager,
            TUser user) where TUser : class
        {
            return new TwoFactorAuthStatusViewModel
            {
                IsEnabled = await userManager.GetTwoFactorEnabledAsync(user),
                HasAuthenticatorKey = await userManager.GetAuthenticatorKeyAsync(user) is not null,
                RememberBrowser = await signInManager.IsTwoFactorClientRememberedAsync(user)
            };
        }
    }
}