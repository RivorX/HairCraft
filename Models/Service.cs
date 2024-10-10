using HAIRCRAFT.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int SalonId { get; set; }
    public Salon Salon { get; set; }
}
