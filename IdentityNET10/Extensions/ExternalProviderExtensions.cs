using IdentityNET10.Models.Enums;

namespace IdentityNET10.Extensions
{
    public static class ExternalProviderExtensions
    {
        /// <summary>
        /// Establece el nombre para mostrar de un proveedor externo.
        /// </summary>
        public static string GetDisplayName(this ExternalProvider provider)
        {
            return provider switch
            {
                ExternalProvider.Facebook => "Facebook",
                ExternalProvider.Google => "Google",
                _ => provider.ToString()
            };
        }
    }
}
