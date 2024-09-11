using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using tjenamannen.Data;
using tjenamannen.Models;
using Exception = System.Exception;

namespace tjenamannen.Services.Rimmaskin
{
	public class RimmaskinService : IRimmaskinService
	{

		private readonly IMemoryCache _cache;
		private readonly ApplicationDbContext _db;
		private readonly ILogger<RimmaskinService> _logger;


		public RimmaskinService(IMemoryCache cache, ApplicationDbContext db, ILogger<RimmaskinService> logger)
		{
			_cache = cache;
			_db = db;
			_logger = logger;
		}

		public IList<string> GetRhymingWords(string word)
		{
			var rhymingWords = new List<string>();

			string vowels = FindVowels(word);

			if (!_cache.TryGetValue("DictionaryList", out List<string> dictionaryList))
			{
				dictionaryList = LoadDictionaryFromDb();
			}


			rhymingWords = dictionaryList.Where(x => FindVowels(x) == vowels).ToList();
			

			return rhymingWords;
		}

		public void UploadWordsFromJson(string word)
		{
			throw new NotImplementedException();
		}

		public void UploadWordsShittyWay()
		{
			List<string> words;
			using (StreamReader r = new StreamReader("Resources/svenska-ord.json"))
			{
				string json = r.ReadToEnd();
				words = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();

				words = words.Select(x => x.ToLower()).ToList();
			}

			foreach (var dbWord in _db.Words)
			{
				if (words.Contains(dbWord.WordId))
				{
					words.Remove(dbWord.WordId);
				}
			}

			foreach (string wordToAdd in words)
			{
				try
				{
					_db.Words.Add(new Word { WordId = wordToAdd });

				}
				catch (Exception e)
				{
					_logger.LogError(e, "Couldn't add word");
				}
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
