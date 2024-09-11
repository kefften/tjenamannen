using System.ComponentModel.DataAnnotations;

namespace tjenamannen.Models.Pages
{
    public class Minecraft
    {
        public List<Player> Players { get; set; }


    }
    public class Player
    {
        [Key]
        public string Uuid { get; set; }
        public string Name { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public List<Achievement>? Achievements { get; set; }
    }

    public class Achievement
    {
        [Key]
        public string Name { get; set; }
    }
}