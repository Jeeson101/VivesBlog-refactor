using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace VivesBlog.Controllers
{
	public class HomeController : Controller
	{
		private readonly DB _database;

		public HomeController()
		{
			var builder = new DbContextOptionsBuilder<DB>();
			builder.UseInMemoryDatabase("VivesBlog");
			_database = new DB(builder.Options);
			if (!_database.Articles.Any())
			{
				_database.Seed();
			}
		}


		public IActionResult Index()
		{
			var articles = _database.Articles
				.Include(a => a.Author)
				.ToList();
			return View(articles);
		}

		public IActionResult Details(int id)
		{
			var article = _database.Articles
				.Include(a => a.Author)
				.SingleOrDefault(a => a.Key == id);

			return View(article);
		}

	}
}
