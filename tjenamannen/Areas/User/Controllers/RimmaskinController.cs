using tjenamannen.Data;
using Microsoft.AspNetCore.Mvc;
using tjenamannen.Models;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace tjenamannen.Areas.User.Controllers
{
    [Area("User")]
    public class RimmaskinController : Controller
    {
        private readonly ILogger<RimmaskinController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache;

        public RimmaskinController(ILogger<RimmaskinController> logger, ApplicationDbContext db, IMemoryCache memoryCache)
        {
            _logger = logger;
            _db = db;
            _cache = memoryCache;
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

            var wordsToAdd = new List<string>();

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

        private string FindVowels(string inputString)
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

            if (!_cache.TryGetValue("DictionaryList", out List<string> dictionaryList))
            {
                dictionaryList = LoadDictionaryFromDb();
            }

            var returnModel = new Rimmaskin
            {
                searchWord = model.searchWord,
                returnWords = dictionaryList.Where(x => FindVowels(x) == vowels).ToList()
            };

            return View("Index", returnModel);
        }

        private List<string> LoadDictionaryFromDb()
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)); // Cache expiration time (5 minutes)

            var dictionaryList = _db.Words.Select(w => w.WordId).ToList();

            _cache.Set("DictionaryList", dictionaryList, cacheOptions);

            return dictionaryList;
        }
    }
}
