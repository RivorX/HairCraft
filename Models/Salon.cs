using System.ComponentModel.DataAnnotations;

namespace HAIRCRAFT.Models
{
    public class Salon
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany.")]
        public string Address { get; set; }

        public string Description { get; set; }

        // Pole OwnerId, które będzie ustawiane automatycznie
        public int OwnerId { get; set; }
    }
}

