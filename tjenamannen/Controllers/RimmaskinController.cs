using tjenamannen.Data;
using Microsoft.AspNetCore.Mvc;
using tjenamannen.Services.Rimmaskin;

namespace tjenamannen.Controllers
{
	public class RimmaskinController : Controller
	{
		private readonly IRimmaskinService _rimService;

		public RimmaskinController(IRimmaskinService rimService)
		{
			_rimService = rimService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Upload()
		{
			return View();
		}

		public void UploadShittyWay()
		{
			_rimService.UploadWordsShittyWay();
		}

		public IActionResult GenerateBar(string word)
		{
			var rhymingWords = _rimService.GetRhymingWords(word);

			return View("Index", rhymingWords);
		}

	}
}
