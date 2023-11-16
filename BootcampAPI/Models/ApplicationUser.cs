using Microsoft.AspNetCore.Identity;

namespace BootcampAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
