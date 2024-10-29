using Microsoft.AspNetCore.Identity;

namespace WebApp.Models
{
    public class ApplicationRole : IdentityRole
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int PermissionsLevel { get; set; }
    }
}
