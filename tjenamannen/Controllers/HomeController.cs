using tjenamannen.Data;
using Microsoft.AspNetCore.Mvc;
using tjenamannen.Models;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace tjenamannen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
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


        



        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}