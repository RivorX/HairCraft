namespace HAIRCRAFT.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public User Client { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }

}
