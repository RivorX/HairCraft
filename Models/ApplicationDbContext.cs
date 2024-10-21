using HAIRCRAFT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Salon> Salons { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Service> Services { get; set; } // Dodanie DbSet dla Service, jeśli jest potrzebny

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ustawienie precyzji dla pola Price w modelu Service
        modelBuilder.Entity<Service>()
            .Property(s => s.Price)
            .HasColumnType("decimal(18,2)"); // Precyzja: 18 cyfr, 2 miejsca po przecinku

        // Ustawienie kluczy obcych dla Appointments
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Client)  // Powiązanie z użytkownikiem (klient)
            .WithMany()
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Restrict); // Zmiana na Restrict (NO ACTION)

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Salon) // Powiązanie z salonem
            .WithMany()
            .HasForeignKey(a => a.SalonId)
            .OnDelete(DeleteBehavior.Cascade); // Pozostawienie kaskadowego usuwania

        // W razie potrzeby, możesz dodać powiązanie z Service
        // modelBuilder.Entity<Appointment>()
        //     .HasOne(a => a.Service) // Zakładając, że masz właściwość Service w Appointment
        //     .WithMany()
        //     .HasForeignKey(a => a.ServiceId) // Musisz mieć ServiceId w Appointment
        //     .OnDelete(DeleteBehavior.Cascade); // Kaskadowe usuwanie
    }
}
