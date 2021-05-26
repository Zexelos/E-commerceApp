using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Domain.Model
{
    public class AppUser : IdentityUser
    {
        public Employee Employee { get; set; }
    }
}