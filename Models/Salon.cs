using System.ComponentModel.DataAnnotations;

namespace HAIRCRAFT.Models
{
    public class Salon
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string Description { get; set; }

        // Właściciel salonu
        [Required]
        public string OwnerId { get; set; }

    }
}
