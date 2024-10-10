using HAIRCRAFT.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Salon> Salons { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

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
            .HasOne(a => a.Service) // Powiązanie z usługą
            .WithMany()
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Cascade); // Pozostawienie kaskadowego usuwania
    }
}
