using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace VivesBlog.Controllers
{
	public class PeopleController : Controller
	{

		private readonly DB _database;

		public PeopleController()
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


		[HttpGet("People/Index")]
		public IActionResult PeopleIndex()
		{
			var people = _database.People.ToList();
			return View(people);
		}

		[HttpGet("People/Create")]
		public IActionResult PeopleCreate()
		{
			return View();
		}

		[HttpPost("People/Create")]
		public IActionResult PeopleCreate(Person person)
		{
			if (!ModelState.IsValid)
			{
				return View(person);
			}
			_database.People.Add(person);
			_database.SaveChanges();

			return RedirectToAction("PeopleIndex");
		}

		[HttpGet("People/Edit/{id}")]
		public IActionResult PeopleEdit(int id)
		{
			var person = _database.People.Single(p => p.Id == id);

			return View(person);
		}

		[HttpPost("People/Edit/{id}")]
		public IActionResult PeopleEdit(Person person)
		{
			if (!ModelState.IsValid)
			{
				return View(person);
			}

			var dbPerson = _database.People.Single(p => p.Id == person.Id);

			dbPerson.Name1 = person.Name1;
			dbPerson.Name2 = person.Name2;

			_database.SaveChanges();

			return RedirectToAction("PeopleIndex");
		}

		[HttpGet("People/Delete/{id}")]
		public IActionResult PeopleDelete(int id)
		{
			var person = _database.People.Single(p => p.Id == id);

			return View(person);
		}

		[HttpPost("People/Delete/{id}")]
		public IActionResult PeopleDeleteConfirmed(int id)
		{
			var dbPerson = _database.People.Single(p => p.Id == id);

			_database.People.Remove(dbPerson);

			_database.SaveChanges();

			return RedirectToAction("PeopleIndex");
		}
	}
}
