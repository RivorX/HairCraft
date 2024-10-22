using Microsoft.AspNetCore.Identity;

namespace HAIRCRAFT.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
