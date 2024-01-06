using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Transactions;

namespace ProductsModule.Models.ViewModels
{
	public class ProductWithCategories
	{
        public int ProductId { get; set; }

		public string ProductName { get; set; }

		public string Description { get; set; }
		public bool IsDeleted { get; set; }

		[Column(TypeName = "Date")]
		[DisplayName("Creation date")]
		public DateTime CreationDate { get; set; }

		[MaxLength(300)]
		public string ImageUrl { get; set; }

		public ICollection<Category> Categories { get; set; } = new List<Category>();
	}
}
