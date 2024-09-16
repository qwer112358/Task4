using Microsoft.AspNetCore.Identity;

namespace UserManagement.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
		public DateTime LastLoginDate { get; set; }
        public bool IsBlocked { get; set; }
    }
}
