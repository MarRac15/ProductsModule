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
			List<Category> objCategoryList = _db.Tb_Categories.ToList();
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
				_db.Tb_Categories.Add(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View();

		}

		public IActionResult Delete()
		{
			return View();
		}

		//Delete POST method


		//GET
		public IActionResult Edit(int? id)
		{
			if (id == null || id==0)
			{
				return NotFound();
			}

			Category? category = _db.Tb_Categories.FirstOrDefault(x=>x.CategoryId==id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		[HttpPost]
		public IActionResult Edit(Category obj)
		{
            if (ModelState.IsValid)
            {
				_db.Tb_Categories.Update(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
            }
			return View(obj);
        }

		
	} 
}
		