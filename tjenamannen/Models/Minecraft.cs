namespace tjenamannen.Models
{
    public class Minecraft
    {
        public List<Player> Players { get; set; }


    }
    public class Player
    {
        public string Name { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
    }
}