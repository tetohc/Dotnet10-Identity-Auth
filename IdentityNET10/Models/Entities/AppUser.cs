using Microsoft.AspNetCore.Identity;

namespace IdentityNET10.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public int CountryCode { get; set; }
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateOnly Birthdate { get; set; }
        public bool IsActive { get; set; }
    }
}