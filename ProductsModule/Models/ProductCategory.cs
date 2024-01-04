namespace ProductsModule.Models
{
	public class ProductCategory
	{
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public int CategoryCategoryId { get; set; }
		public Category Category { get; set; }

	}
}
