﻿using System;
using Microsoft.AspNetCore.Identity;

namespace HAIRCRAFT.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public IdentityUser Client { get; set; }

        public string Service { get; set; }
        public DateTime AppointmentDate { get; set; }

        // Klucz obcy do salonu
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        // Pole do przechowywania oceny wizyty (1-5)
        public int? Rating { get; set; } // Używamy int?, aby wartość mogła być null, gdy jeszcze nie ma oceny
    }
}
