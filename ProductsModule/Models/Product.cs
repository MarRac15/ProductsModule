using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsModule.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }

		[Column(TypeName = "Date")]
		[DisplayName("Creation date")]
		public DateTime CreationDate { get; set; }

		[MaxLength(300)]
		public string ImageUrl { get; set; }
		public ICollection<ProductCategory> Categories { get; set; } = new List<ProductCategory>();

		public int SelectedCategoryId { get; set; }
	}
}


