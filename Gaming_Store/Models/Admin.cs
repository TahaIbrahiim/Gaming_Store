namespace Gaming_Store.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime Birthdate { get; set; }
        public string Country { get; set; }
        public byte[]? Image { get; set; }


    }
}
