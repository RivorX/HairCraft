namespace HAIRCRAFT.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }  // Powiązanie z fryzjerem
        public User Owner { get; set; }
    }

}
