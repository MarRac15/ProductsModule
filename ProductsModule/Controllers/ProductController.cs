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
			//List<Product> objProductList = _db.Products.Where(b => !b.IsDeleted).ToList();
			//var productsWithCategories = _db.Products.Include(x => x.Categories).Where(x=>!x.IsDeleted).ToList();

			var productsWithCategories = _db.Products
			.Include(p => p.Categories)
			.Select(p => new ProductWithCategories
			{
				ProductId = p.Id,
				ProductName = p.Title,
				Description = p.Description,
				ImageUrl = p.ImageUrl,
				CreationDate = p.CreationDate,
				IsDeleted = p.IsDeleted,
				Categories = p.Categories.Select(c => c.Category).ToList()
			})
			.Where(b => !b.IsDeleted).ToList();


			return View(productsWithCategories);
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
		public IActionResult Create(ProductVM obj)
		{
			if (ModelState.IsValid)
			{
				if (obj.Product.SelectedCategoryId != null)
				{

					var selectedCategory = _db.Categories.Find(obj.Product.SelectedCategoryId);

					obj.Product.Categories = new List<ProductCategory> { new ProductCategory { CategoryCategoryId = obj.Product.SelectedCategoryId, Category = selectedCategory } };


					_db.Products.Add(obj.Product);
					_db.SaveChanges();
					TempData["success"] = "(Product successfully created!)";
						
					return RedirectToAction("Index");
				}
			}
			
			

				obj.CategoryList = _db.Categories.Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.CategoryId.ToString()
				}).ToList();

				return View(obj);
			

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
