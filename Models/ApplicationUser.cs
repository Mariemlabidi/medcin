using Microsoft.AspNetCore.Identity;

namespace medcin.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
