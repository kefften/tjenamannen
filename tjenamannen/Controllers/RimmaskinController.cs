using tjenamannen.Data;
using Microsoft.AspNetCore.Mvc;
using tjenamannen.Models;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace tjenamannen.Controllers
{
    public class RimmaskinController : Controller
    {
        private readonly ILogger<RimmaskinController> _logger;
        private ApplicationDbContext _db;
        public RimmaskinController(ILogger<RimmaskinController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public void UploadFileToDb()
        {
            IEnumerable<string> dictionary = new List<string>();
            using (StreamReader r = new StreamReader("Resources/svenska-ord.json"))
            {
                string json = r.ReadToEnd();
                dictionary = JsonSerializer.Deserialize<List<string>>(json);
            }
            List<string> wordsToAdd = new List<string>();

            foreach (string word in dictionary)
            {
                if (!wordsToAdd.Contains(word.ToLower()))
                {
                    wordsToAdd.Add(word.ToLower());
                }
                
            }
            foreach (string word2add in wordsToAdd)
            {
                _db.Words.Add(new Word { WordId = word2add });
                
            }
            _db.SaveChanges();
        }

        private string FindVowels(String inputString)
        {
            string vowelWord = string.Empty;

            if (inputString != null)
            {
                var vowels = new List<char> { 'a', 'o', 'u', 'å', 'e', 'i', 'y', 'ä', 'ö' };
                foreach (char c in inputString.ToLower())
                {
                    if (vowels.Contains(c))
                    {
                        vowelWord += c;
                    }
                }
            }

            return vowelWord;
        }

        [HttpPost]
        public IActionResult GenerateBar(Rimmaskin model)
        {
            string vowels = FindVowels(model.searchWord);
            //var dictionary = new List<string> {"Menade", "Stenade", "Hejsan", "Varför", "Alltid", "Mycket"};

            //string jsonData = File.ReadAllText("data.json");

            //string jsonData = File.ReadAllText("data.json");
            List<string> dictionary2send = new List<string>();

           
            //using (StreamReader r = new StreamReader("Resources/svenska-ord.json"))
            //{
            //    string json = r.ReadToEnd();
            //    dictionary = JsonSerializer.Deserialize<List<string>>(json);
            //}


            var dictionary = _db.Words;

            foreach(var word in dictionary)
            {
                dictionary2send.Add(word.WordId);
            }


            //List<string> wordsList = returnWords.Select(s => s.WordId).ToList<string>();
 
            var returnModel = new Rimmaskin
            {
                searchWord = model.searchWord,
                returnWords = dictionary2send.Where(x => FindVowels(x) == vowels).ToList()
            };
            //if (ModelState.IsValid)
            //{

            //}
            return View("Index", returnModel);
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}