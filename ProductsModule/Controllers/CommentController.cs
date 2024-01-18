using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
			IEnumerable<SelectListItem> CommentList = _db.Products.Where(c => !c.IsDeleted).ToList().Select(u => new SelectListItem
			{
				Text = u.Title,
				Value = u.Id.ToString()
			});

			ViewData["CommentList"] = CommentList;

			return View();
		}

		[HttpPost]
		public IActionResult Create(Comment obj, int ProductId)
		{
			var selectedProduct = _db.Products.FirstOrDefault(c=>c.Id == ProductId);
			
			obj.Product = selectedProduct;

			_db.Comment.Add(obj);
			_db.SaveChanges();
			TempData["success"] = "(Comment successfully created!)";
			return RedirectToAction("Index");

		}


		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Comment? commentFromDb = _db.Comment.FirstOrDefault(x => x.Id == id);
			if (commentFromDb == null)
			{
				return NotFound();
			}

			return View(commentFromDb);
		}


		[HttpPost]
		[ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Comment? obj = _db.Comment.Find(id);
			if (obj == null)
			{
				return NotFound();
			}

			obj.IsDeleted = true;
			_db.SaveChanges();
			TempData["success"] = "(Comment successfully deleted!)";
			return RedirectToAction("Index");

		}

		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Comment? commentFromDb = _db.Comment.FirstOrDefault(x => x.Id == id);
			if (commentFromDb == null)
			{
				return NotFound();
			}

			return View(commentFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Comment obj, int ProductId)
		{
			var selectedProduct = _db.Products.FirstOrDefault(c => c.Id == ProductId);

			obj.Product = selectedProduct;

			_db.Comment.Update(obj);
			_db.SaveChanges();
			
			return RedirectToAction("Index");

		}


	}
}
