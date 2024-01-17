using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsModule.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public DateTime CreationDate { get; set; }
		public bool IsDeleted { get; set; }

		
		public int ProductId { get; set; }
	}
}
