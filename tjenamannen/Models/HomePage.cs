namespace tjenamannen.Models
{
    public class HomePage
    {
        public string searchWord { get; set; }
        public List<string> returnWords { get; set; }
        public List<Word> Words { get; set; }


        public class Word
        {
            public string word { get; set; }
        }
    }
}