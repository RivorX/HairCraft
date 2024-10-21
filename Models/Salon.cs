using Microsoft.AspNetCore.Identity;

namespace HAIRCRAFT.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        // Powiązanie z tożsamością użytkownika
        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }
    }
}
