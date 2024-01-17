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
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
		{
			_db = db;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index(string searchString, int pageNumber)
		{
			ViewData["CurrentFilter"] = searchString;

			var query = _db.Products
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
			.Where(b => !b.IsDeleted);

			//searching:
			
			if (!String.IsNullOrEmpty(searchString))
			{
				query = query.Where(p => p.ProductName.Contains(searchString));
			}

			//paging:
			int pageSize = 3;
			

			var productsWithCategories = query.ToList();

			return View(PageList<ProductWithCategories>.Create(productsWithCategories, pageNumber, pageSize));
		}

		public IActionResult Create()
		{

			ProductVM productVM = new ProductVM()
			{
				CategoryList = _db.Categories.Where(c=>!c.IsDeleted).ToList().Select(u => new SelectListItem
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

					
					if (obj.ImageFile != null)
					{
						string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
						string fileName = Guid.NewGuid().ToString() + "_" + obj.ImageFile.FileName;
						string filePath = Path.Combine(uploadFolder, fileName);

						using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							obj.ImageFile.CopyTo(fileStream);
						}

						obj.Product.ImageUrl = "/images/" + fileName;
					}
					
					_db.Products.Add(obj.Product);
					_db.SaveChanges();
					TempData["success"] = "(Product successfully created!)";
						
					return RedirectToAction("Index");
				}
			}
			//jakby modelState był false to sprawdzam co jest nie tak:
			foreach (var modelStateKey in ModelState.Keys)
			{
				var modelStateVal = ModelState[modelStateKey];
				if (modelStateVal.Errors.Count > 0)
				{
					foreach (var error in modelStateVal.Errors)
					{
						Console.WriteLine($"Błąd walidacji dla {modelStateKey}: {error.ErrorMessage}");
					}
				}
			}
			obj.CategoryList = _db.Categories.Where(c => !c.IsDeleted).Select(u => new SelectListItem
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

			var existingProduct = _db.Products.Include(p => p.Categories).FirstOrDefault(p => p.Id == id);
			ProductVM productVM = new ProductVM()
			{
				CategoryList = _db.Categories.Where(c => !c.IsDeleted).ToList().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.CategoryId.ToString()
				}),
				Product = _db.Products.FirstOrDefault(p=>p.Id==id),
		};
			

			if (productVM == null)
			{
				return NotFound();
			}

			return View(productVM);
		}

		[HttpPost]
		public IActionResult Edit(ProductVM obj)
		{
			if (ModelState.IsValid)
			{
				var existingProduct = _db.Products.Include(p => p.Categories).FirstOrDefault(p => p.Id == obj.Product.Id);

				existingProduct.Categories.Clear();

				if (obj.Product.SelectedCategoryId!=null) 
				{
					var selectedCategory = _db.Categories.Find(obj.Product.SelectedCategoryId);

					existingProduct.Categories.Add(new ProductCategory { CategoryCategoryId = obj.Product.SelectedCategoryId, Category = selectedCategory });
					existingProduct.Title=obj.Product.Title;
					existingProduct.Description = obj.Product.Description;
					
					existingProduct.CreationDate = obj.Product.CreationDate;

					if (obj.ImageFile != null)
					{
						string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
						string fileName = Guid.NewGuid().ToString() + "_" + obj.ImageFile.FileName;
						string filePath = Path.Combine(uploadFolder, fileName);

						using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							obj.ImageFile.CopyTo(fileStream);
						}

						obj.Product.ImageUrl = "/images/" + fileName;
						existingProduct.ImageUrl = obj.Product.ImageUrl;

					}
					
					_db.SaveChanges();
					return RedirectToAction("Index");
				}
				
			}

			obj.CategoryList = _db.Categories.Where(c => !c.IsDeleted).Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.CategoryId.ToString()
			}).ToList();

			return View(obj);
		}
	}
}
