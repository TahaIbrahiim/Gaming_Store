namespace Gaming_Store.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }  // PC or PS
        public string MoodType { get; set; }  // online or offline or Both
        public string Category { get; set; }  // Horror - Action - Story
        public string url { get; set; }  
        public DateTime Date { get; set; }
        public byte[]? Image { get; set; }
        public byte[]? Video { get; set; }

        public ICollection<ClientGame> ClientGames { get; set; } = new List<ClientGame>();


            
            
            
                

    }
}
