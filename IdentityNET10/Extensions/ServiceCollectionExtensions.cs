using IdentityNET10.Data;
using IdentityNET10.Models.Entities;
using IdentityNET10.Services;
using IdentityNET10.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace IdentityNET10.Extensions
{
    /// <summary>
    /// Esta clase contiene métodos de extensión para IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Agrega el servicio de persistencia y configura el DbContext con SQL Server.
        /// </summary>
        /// <param name="services">Colección de servicios de la aplicación.</param>
        /// <param name="configuration">Configuración de la aplicación que contiene la cadena de conexión.</param>
        /// <returns>La colección de servicios actualizada con el DbContext registrado.</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("SQLConnectionString")
                ),
                ServiceLifetime.Transient
            );
            return services;
        }

        /// <summary>
        /// Agrega y configura los servicios de Identity en la aplicación.
        /// </summary>
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // opciones de contraseña
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // opciones de bloqueo
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // opciones de usuario
                options.User.RequireUniqueEmail = true;

                // opciones de inicio de sesión
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
            });
            return services;
        }

        /// <summary>
        /// Configura las opciones de la cookie de autenticación de la aplicación.
        /// </summary>
        public static IServiceCollection AddAuthenticationCookieSettings(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;

                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";

                options.ReturnUrlParameter = "ReturnUrl";

                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "IdentityAppAuthCookie";
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            return services;
        }

        /// <summary>
        /// Registra la configuración de EmailSettings usando el Options Pattern.
        /// </summary>
        public static IServiceCollection AddEmailSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            return services;
        }

        /// <summary>
        /// Registra los servicios de la aplicación.
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            return services;
        }
    }
}