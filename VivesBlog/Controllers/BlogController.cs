using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace VivesBlog.Controllers
{
	public class BlogController : Controller
	{
		private readonly DB _database;

		public BlogController()
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
			return View();
		}


		[HttpGet("Blog/Index")]
		public IActionResult BlogIndex()
		{
			var articles = _database.Articles
				.Include(a => a.Author)
				.ToList();
			return View(articles);
		}

		[HttpGet("Blog/Create")]
		public IActionResult BlogCreate()
		{
			var articleModel = CreateArticleModel();

			return View(articleModel);
		}

		[HttpPost("Blog/Create")]
		public IActionResult BlogCreate(Article article)
		{
			if (!ModelState.IsValid)
			{
				var articleModel = CreateArticleModel(article);
				return View(articleModel);
			}

			article.CreatedDate = DateTime.Now;

			_database.Articles.Add(article);

			_database.SaveChanges();

			return RedirectToAction("BlogIndex");
		}

		[HttpGet("Blog/Edit/{id}")]
		public IActionResult BlogEdit(int id)
		{
			var article = _database.Articles.Single(p => p.Key == id);

			var articleModel = CreateArticleModel(article);

			return View(articleModel);
		}

		[HttpPost("Blog/Edit/{id}")]
		public IActionResult BlogEdit(Article article)
		{
			if (!ModelState.IsValid)
			{
				var articleModel = CreateArticleModel(article);
				return View(articleModel);
			}

			var dbArticle = _database.Articles.Single(p => p.Key == article.Key);

			dbArticle.Title = article.Title;
			dbArticle.Description = article.Description;
			dbArticle.Content = article.Content;
			dbArticle.AuthorId = article.AuthorId;

			_database.SaveChanges();

			return RedirectToAction("BlogIndex");
		}

		[HttpGet("Blog/Delete/{id}")]
		public IActionResult BlogDelete(int id)
		{
			var article = _database.Articles
				.Include(a => a.Author)
				.Single(p => p.Key == id);

			return View(article);
		}

		[HttpPost("Blog/Delete/{id}")]
		public IActionResult BlogDeleteConfirmed(int id)
		{
			var dbArticle = _database.Articles.Single(p => p.Key == id);

			_database.Articles.Remove(dbArticle);

			_database.SaveChanges();

			return RedirectToAction("BlogIndex");
		}

		private ArticleModel CreateArticleModel(Article article = null)
		{
			article ??= new Article();

			var authors = _database.People
				.OrderBy(a => a.Name1)
				.ThenBy(a => a.Name2)
				.ToList();

			var articleModel = new ArticleModel
			{
				Article = article,
				Authors = authors
			};

			return articleModel;
		}
	}
}
