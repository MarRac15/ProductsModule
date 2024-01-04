using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProductsModule.Models;

namespace ProductsModule.Data
{
	public class AppDbContext: DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }

		public DbSet<ProductCategory> ProductCategories { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<ProductCategory>()
				.HasKey(pc => new { pc.ProductId, pc.CategoryCategoryId });

			modelBuilder.Entity<ProductCategory>()
				.HasOne(pc => pc.Product)
				.WithMany(pc => pc.Categories)
				.HasForeignKey(pc => pc.ProductId);

			modelBuilder.Entity<ProductCategory>()
				.HasOne(pc => pc.Category)
				.WithMany(pc => pc.Products)
				.HasForeignKey(pc => pc.CategoryCategoryId);



			modelBuilder.Entity<Category>().HasData(
				new Category { CategoryId = 1, Name = "Food", IsDeleted = false },
                new Category { CategoryId = 2, Name = "Toys", IsDeleted = false },
                new Category { CategoryId = 3, Name = "Tech", IsDeleted = false }
                );

			modelBuilder.Entity<Product>().Property(e => e.CreationDate).HasColumnType("date");
			modelBuilder.Entity<Product>().HasData(
				new Product
				{
					Id=1,
					Title="The Witcher",
					Description="epic book",
					IsDeleted=false,
					CreationDate=new DateTime(),
					ImageUrl="",
				},
				new Product
				{
					Id = 2,
					Title = "Fanta",
					Description = "tasty drink",
					IsDeleted = false,
					CreationDate = new DateTime(),
					ImageUrl = "",
				},
				new Product
				{
					Id = 3,
					Title = "Ginger Bread",
					Description = "Kopernik's finest biscuits",
					IsDeleted = false,
					CreationDate = new DateTime(),
					ImageUrl = "",
				}
				);


	}
	}
}
