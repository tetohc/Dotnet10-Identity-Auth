namespace IdentityNET10.Settings
{
    /// <summary>
    /// Representa la configuración de correo electrónico.
    /// </summary>
    public class EmailSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}