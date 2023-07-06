using tjenamannen.Data;
using Microsoft.AspNetCore.Mvc;
using tjenamannen.Models;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.IO;

namespace tjenamannen.Controllers
{
    public class MinecraftController : Controller
    {
        private readonly ILogger<RimmaskinController> _logger;
        private ApplicationDbContext _db;
        public MinecraftController(ILogger<RimmaskinController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
		private string ReadFileContents(string filePath)
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

		private Minecraft MinecraftBuilder()
		{
			string filePath = @"C:\_DEV\Minecraft Server\logs\latest.log".Replace(@"\\", @"\");
			string fileContent = ReadFileContents(filePath);

			var playerNames = fileContent.Split("[Server thread/INFO]: ").Where(x => x.Split(' ')[1] == "joined").Select(x => x.Split(' ').FirstOrDefault()).Distinct();
			var players = new List<Player>();
			
			foreach (var playerName in playerNames) 
			{
				players.Add(new Player { Name = playerName });
			}

			Console.WriteLine(fileContent);

			var model = new Minecraft();
			model.Players = players;


			return model;
		}

		public IActionResult Index()
        {
			var model = MinecraftBuilder();
			return View(model);
        }

    }
}