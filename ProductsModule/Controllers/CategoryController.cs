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
	}
}
