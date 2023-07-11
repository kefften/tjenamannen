using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using fNbt;
using Microsoft.Extensions.Caching.Memory;
using tjenamannen.Data;
using tjenamannen.Models;

namespace tjenamannen.Controllers
{
	public class MinecraftController : Controller
	{
		private readonly ILogger<MinecraftController> _logger;
		private readonly ApplicationDbContext _db;
		private readonly IMemoryCache _cache;
		private readonly string _logPath = @"C:\_DEV\Minecraft Server\logs\latest.log";
		private readonly string _playerDatPath = @"C:\_DEV\Minecraft Server\tjenamannen\playerdata\";
		private readonly string _playerCacheJsonPath = @"C:\_DEV\Minecraft Server\usercache.json";

		public MinecraftController(ILogger<MinecraftController> logger, ApplicationDbContext db, IMemoryCache memoryCache)
		{
			_logger = logger;
			_db = db;
			_cache = memoryCache;
		}

		private string ReadTextFile(string filePath)
		{
			try
			{
				return System.IO.File.ReadAllText(filePath);
			}
			catch (FileNotFoundException)
			{
				_logger.LogError("File not found: {FilePath}", filePath);
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred: {ErrorMessage}", ex.Message);
			}

			return string.Empty;
		}

		private List<Player> GetPlayersFromJson(string filePath)
		{
			if (_cache.TryGetValue("PlayersFromJson", out List<Player> players))
			{
				return players;
			}

			try
			{
				string json = ReadTextFile(filePath);

				if (!string.IsNullOrEmpty(json))
				{
					var data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

					players = data.Select(item => new Player
					{
						Name = item["name"],
						Uuid = item["uuid"]
					}).ToList();

					var cacheOptions = new MemoryCacheEntryOptions()
						.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

					_cache.Set("PlayersFromJson", players, cacheOptions);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred while getting players from JSON: {ErrorMessage}", ex.Message);
			}

			return players ?? new List<Player>();
		}

		private List<NbtTag> ReadUserCacheFromDatFile(string userId)
		{
			string filePath = Path.Combine(_playerDatPath, $"{userId}.dat");

			if (!System.IO.File.Exists(filePath))
			{
				return new List<NbtTag>();
			}

			try
			{
				var myFile = new NbtFile();
				myFile.LoadFromFile(filePath);
				return myFile.RootTag.Tags.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred while reading user cache from DAT file: {ErrorMessage}", ex.Message);
			}

			return new List<NbtTag>();
		}

		private void RefreshPlayersInDatabase(List<Player> players)
		{
			foreach (var player in players)
			{
				if (!_db.Players.Any(p => p.Uuid == player.Uuid))
				{
					_db.Players.Add(player);
				}
			}

			_db.SaveChanges();
		}

		private Minecraft MinecraftBuilder()
		{
			var players = GetPlayersFromJson(_playerCacheJsonPath);
			var playersCache = players.Select(x => ReadUserCacheFromDatFile(x.Uuid)).ToList();

			RefreshPlayersInDatabase(players);

			var model = new Minecraft
			{
				Players = players
			};

			return model;
		}

		public IActionResult Index()
		{
			var model = MinecraftBuilder();
			return View(model);
		}
	}
}
