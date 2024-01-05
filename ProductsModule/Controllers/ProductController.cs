using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductsModule.Data;
using ProductsModule.Models;
using ProductsModule.Models.ViewModels;
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
			List<Product> objProductList = _db.Products.Where(b => !b.IsDeleted).ToList();
			

			return View(objProductList);
		}

		public IActionResult Create()
		{

			ProductVM productVM = new ProductVM()
			{
				CategoryList = _db.Categories.ToList().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.CategoryId.ToString()
				}),
				Product = new Product()
			};
			return View(productVM);
		}

		[HttpPost]
		public IActionResult Create(ProductVM obj, int selectedCategoryId)
		{
			if (ModelState.IsValid)
			{
				var categoriesInfo = _db.Categories.SingleOrDefault(c => c.CategoryId == selectedCategoryId);

				_db.Products.Add(obj.Product);
				_db.SaveChanges();
				TempData["success"] = "(Product successfully created!)";
				return RedirectToAction("Index");
			}
			else
			{

				obj.CategoryList = _db.Categories.ToList().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.CategoryId.ToString()
				});

				return View(obj);
			}

		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Product? productFromDb = _db.Products.FirstOrDefault(x => x.Id == id);
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
			Product? obj = _db.Products.Find(id);
			if (obj==null)
			{
				return NotFound();
			}
			obj.IsDeleted = true;
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

			Product? productFromDb = _db.Products.Find(id);
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
				_db.Products.Update(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(obj);
		}
	}
}
