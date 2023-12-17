using Microsoft.AspNetCore.Mvc;
using ProductsModule.Data;
using ProductsModule.Models;
using System.ComponentModel;

namespace ProductsModule.Controllers
{
	
	public class ProductController : Controller
	{
		private readonly AppDbContext _db;

		public ProductController(AppDbContext db)
		{
			_db = db;
		}
		public IActionResult Index()
		{
			List<Product> objProductList = _db.Tb_Products.ToList();
			return View(objProductList);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Product obj)
		{
			if (ModelState.IsValid)
			{
				_db.Tb_Products.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "(Product successfully created!)";
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

			Product? productFromDb = _db.Tb_Products.FirstOrDefault(x => x.Id == id);
			if (productFromDb == null)
			{
				return NotFound();
			}

			return View(productFromDb);
		}

		[HttpPost]
		[ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Product? obj = _db.Tb_Products.Find(id);
			if (obj==null)
			{
				return NotFound();
			}
			_db.Tb_Products.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "(Product successfully deleted!)";
			return RedirectToAction("Index");
		}


		//GET
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Product? productFromDb = _db.Tb_Products.Find(id);
			if (productFromDb == null)
			{
				return NotFound();
			}

			return View(productFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Product obj)
		{
			if (ModelState.IsValid)
			{
				_db.Tb_Products.Update(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(obj);
		}
	}
}
