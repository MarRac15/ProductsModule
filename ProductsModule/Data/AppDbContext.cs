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

		public DbSet<Category> Category { get; set; }
		public DbSet<Product> Product { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

			
		
			modelBuilder.Entity<Product>()
				.HasMany(e => e.Categories)
				.WithMany(e => e.Products)
				.UsingEntity(j=>j.ToTable("ProductCategory"));
		

	}
	}
}
