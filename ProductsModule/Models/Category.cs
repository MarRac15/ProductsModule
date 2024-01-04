using System.ComponentModel.DataAnnotations;

namespace ProductsModule.Models
{
	public class Category
	{

		[Key]
		public int CategoryId { get; set; }

		[Required]
		[MaxLength(30)]
		public string Name { get; set; }

		public bool IsDeleted { get; set; }

		public ICollection<ProductCategory> Products { get; set; }
	}
}
