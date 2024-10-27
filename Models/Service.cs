using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace HAIRCRAFT.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nazwa usługi jest wymagana.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Cena usługi jest wymagana.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Salon jest wymagany.")]
        public int SalonId { get; set; }
        [ValidateNever]
        public virtual Salon Salon { get; set; }

    }

}
