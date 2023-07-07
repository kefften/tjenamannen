using tjenamannen.Data;
using Microsoft.AspNetCore.Mvc;
using tjenamannen.Models;
using fNbt;
using Newtonsoft.Json;

namespace tjenamannen.Controllers
{
    public class MinecraftController : Controller
    {
        private readonly ILogger<RimmaskinController> _logger;
        private ApplicationDbContext _db;
		private string _logPath = @"C:\_DEV\MinecraftServer\logs\latest.log".Replace(@"\\", @"\");
		private string _playerDatPath = @"C:\_DEV\MinecraftServer\tjenamannen\playerdata\2b888c92-58b0-4960-8a6e-741ee1efb9b8.dat".Replace(@"\\", @"\");
		private string _playerCacheJsonPath = @"C:\_DEV\MinecraftServer\usercache.json".Replace(@"\\", @"\");
        public MinecraftController(ILogger<RimmaskinController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
		private string ReadTxtFile(string filePath)
		{
			try
			{
				using (StreamReader reader = new StreamReader(filePath))
				{
					string fileContent = reader.ReadToEnd();
					return fileContent;
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("File not found: " + filePath);
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);
			}

			return string.Empty; // Return an empty string if an error occurred
		}

        private List<Player> GetPlayersFromLog(string filePath)
        {
            var players = new List<Player>();

            if (!string.IsNullOrEmpty(filePath))
            {
                var playerNames = filePath.Split("[Server thread/INFO]: ").Where(x => x.Split(' ')[1] == "joined").Select(x => x.Split(' ').FirstOrDefault()).Distinct();
                foreach (var playerName in playerNames)
                {
                    players.Add(new Player { Name = playerName });
                }
            }
            return players;
        }

        private List<Player> GetPlayersFromJson(string filePath)
        {

            string json = System.IO.File.ReadAllText(filePath);

            var players = new List<Player>();
            var data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

            foreach (var item in data)
            {
                players.Add(new Player
                {
                    Name = item["name"],
                    Uuid = item["uuid"]
                });
            }
            return players;
        }

        private string ReadBinaryFile(string playerDatPath)
        {
            if (!System.IO.File.Exists(playerDatPath))
            {
                return string.Empty;
            }

            var myFile = new NbtFile();
            myFile.LoadFromFile(playerDatPath);
            var myCompoundTag = myFile.RootTag;

            var contentTag = myCompoundTag.Get<NbtString>("content");

            if (contentTag == null)
            {
                return string.Empty;
            }

            return contentTag.Value;
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
            //string logFile = ReadTxtFile(_logPath);
            //string playerDatFile = ReadBinaryFile(_playerDatPath);
            var players = GetPlayersFromJson(_playerCacheJsonPath);

            RefreshPlayersInDatabase(players);

            var model = new Minecraft()
            {
                //Players = GetPlayersFromLog(logFile)
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