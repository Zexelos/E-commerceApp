using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public virtual Employee Employee { get; set; }

        public virtual Customer Customer { get; set; }
    }
}