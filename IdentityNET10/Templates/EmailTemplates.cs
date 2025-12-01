namespace IdentityNET10.Templates
{
    /// <summary>
    /// Contiene plantillas de correo electrónico en formato HTML.
    /// </summary>
    public static class EmailTemplates
    {
        /// <summary>
        /// Genera una estructura HTML para el correo de restablecimiento de contraseña.
        /// </summary>
        public static string ForgotPasswordTemplate(string resetUrl)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""es"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f3f6f9;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: 40px auto;
                                background: #ffffff;
                                border-radius: 8px;
                                padding: 30px;
                                box-shadow: 0 4px 12px rgba(0,0,0,0.08);
                            }}
                            h2 {{
                                color: #0d6efd;
                                margin-bottom: 20px;
                            }}
                            p {{
                                color: #555;
                                line-height: 1.6;
                            }}
                            .btn {{
                                display: inline-block;
                                padding: 12px 20px;
                                background-color: #0d6efd;
                                color: #ffffff !important;
                                text-decoration: none;
                                border-radius: 6px;
                                margin-top: 20px;
                                font-weight: bold;
                            }}
                            .footer {{
                                margin-top: 30px;
                                font-size: 12px;
                                color: #999;
                                text-align: center;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                            <h2>Restablecer contraseña</h2>
                            <p>Hemos recibido una solicitud para restablecer tu contraseña.</p>
                            <p>Si tú no realizaste esta solicitud, puedes ignorar este mensaje.</p>
                            <p>Para continuar, haz clic en el siguiente botón:</p>
                            <a href=""{resetUrl}"" class=""btn"">Restablecer contraseña</a>
                            <p class=""footer"">
                                © {DateTime.Now.Year} IdentityNET10 — Este es un mensaje automático, por favor no respondas.
                            </p>
                        </div>
                    </body>
                </html>";
        }

        /// <summary>
        /// Genera una estructura HTML para el correo de confirmación de email.
        /// </summary>
        /// <returns></returns>
        public static string ConfirmEmailTemplate(string confirmUrl, string username)
        {
            return $@"
            <!DOCTYPE html>
            <html lang=""es"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f3f6f9;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 40px auto;
                            background: #ffffff;
                            border-radius: 8px;
                            padding: 30px;
                            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
                        }}
                        h2 {{
                            color: #0d6efd;
                            margin-bottom: 20px;
                        }}
                        p {{
                            color: #555;
                            line-height: 1.6;
                        }}
                        .btn {{
                            display: inline-block;
                            padding: 12px 20px;
                            background-color: #0d6efd;
                            color: #ffffff !important;
                            text-decoration: none;
                            border-radius: 6px;
                            margin-top: 20px;
                            font-weight: bold;
                        }}
                        .footer {{
                            margin-top: 30px;
                            font-size: 12px;
                            color: #999;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <h2>Confirma tu correo electrónico</h2>
                        <p>Hola ""{username}"", gracias por registrarte en <strong>IdentityNET10</strong>.</p>
                        <p>Para activar tu cuenta y comenzar a utilizar la aplicación, por favor confirma tu correo electrónico haciendo clic en el siguiente botón:</p>
                        <a href=""{confirmUrl}"" class=""btn"">Confirmar correo</a>
                        <p>Si tú no creaste esta cuenta, puedes ignorar este mensaje.</p>
                        <p class=""footer"">
                            © {DateTime.Now.Year} IdentityNET10 — Este es un mensaje automático, por favor no respondas.
                        </p>
                    </div>
                </body>
            </html>";
        }
    }
}
