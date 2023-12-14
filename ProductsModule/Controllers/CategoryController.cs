using Microsoft.AspNetCore.Mvc;

namespace ProductsModule.Controllers
{
	public class CategoryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
