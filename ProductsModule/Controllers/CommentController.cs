using Microsoft.AspNetCore.Mvc;
using ProductsModule.Data;
using ProductsModule.Models;

namespace ProductsModule.Controllers
{
	public class CommentController : Controller
	{
		private readonly AppDbContext _db;
		public CommentController(AppDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			List<Comment> objCommentList = _db.Comment.Where(c=>!c.IsDeleted).ToList();
			return View(objCommentList);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Comment obj)
		{
			if (ModelState.IsValid)
			{
				_db.Comment.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "(Comment successfully created!)";
				return RedirectToAction("Index");
			}

			foreach (var modelState in ModelState.Values)
			{
				foreach (var error in modelState.Errors)
				{
					Console.WriteLine(error.ErrorMessage);
				}
			}
			return View();

		}
	}
}
