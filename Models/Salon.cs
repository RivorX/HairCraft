using System.ComponentModel.DataAnnotations;

namespace HAIRCRAFT.Models
{
    public class Salon
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nazwa salonu jest wymagana.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Adres salonu jest wymagany.")]
        public string Address { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public ICollection<Service> Services { get; set; } = new List<Service>(); // Dodajemy kolekcję usług
    }
}
