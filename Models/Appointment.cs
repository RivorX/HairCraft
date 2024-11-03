using System;
using Microsoft.AspNetCore.Identity;

namespace HAIRCRAFT.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public IdentityUser Client { get; set; }

        // Właściwość 'Service' jako string
        public string Service { get; set; }

        public DateTime AppointmentDate { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
    }
}
