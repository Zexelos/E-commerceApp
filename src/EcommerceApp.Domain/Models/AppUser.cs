using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public Employee Employee { get; set; }
    }
}