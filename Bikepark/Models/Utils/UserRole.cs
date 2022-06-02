using Microsoft.AspNetCore.Identity;

namespace Bikepark.Models
{
    public class UserRole
    {
        public IdentityUser User { get; set; }

        public bool IsAdmin { get; set; }
    }
}
