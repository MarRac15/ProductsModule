using System.ComponentModel.DataAnnotations;

namespace ProductsModule.Models
{
	public class Category
	{

		[Key]
		public int CategoryId { get; set; }

		public string Name { get; set; }

		public bool IsDeleted { get; set; }
	}
}
