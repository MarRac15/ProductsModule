using Microsoft.AspNetCore.Mvc;
using ProductsModule.Data;
using ProductsModule.Models;

namespace ProductsModule.Controllers
{
	public class CategoryController : Controller
	{
		private readonly AppDbContext _db;
		public CategoryController(AppDbContext db)
		{
			_db = db;
		}
		public IActionResult Index()
		{
			List<Category> objCategoryList = _db.Category.ToList();
			return View(objCategoryList);
		}

		//GET
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Category obj)
		{
			if (ModelState.IsValid)
			{
				_db.Category.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "(Category successfully created!)";
				return RedirectToAction("Index");
			}
			return View();

		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Category? categoryFromDb = _db.Category.FirstOrDefault(x => x.CategoryId == id);
			if (categoryFromDb == null)
			{
				return NotFound();
			}

			return View(categoryFromDb);
		}

		
		[HttpPost]
		[ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Category? obj = _db.Category.Find(id);
			if (obj==null)
			{
				return NotFound();
			}

			_db.Category.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "(Category successfully deleted!)";
			return RedirectToAction("Index");
			
		}



		//GET
		public IActionResult Edit(int? id)
		{
			if (id == null || id==0)
			{
				return NotFound();
			}

			Category? categoryFromDb = _db.Category.FirstOrDefault(x=>x.CategoryId==id);
			if (categoryFromDb == null)
			{
				return NotFound();
			}

			return View(categoryFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Category obj)
		{
            if (ModelState.IsValid)
            {
				_db.Category.Update(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
            }
			return View(obj);
        }

		
	} 
}
		