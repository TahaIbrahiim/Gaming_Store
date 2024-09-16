using Gaming_Store.Models;

namespace Gaming_Store.ViewModels
{
    public class GameDetailsVM
    {
        public Game Game { get; set; }
        public bool IsEnrolled { get; set; }
        public string UserRole { get; set; }
    }
}
