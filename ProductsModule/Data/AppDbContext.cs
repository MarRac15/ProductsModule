using Microsoft.EntityFrameworkCore;
using ProductsModule.Models;

namespace ProductsModule.Data
{
	public class AppDbContext: DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Tb_Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
				new Category { CategoryId = 1, Name = "Food", IsDeleted = false },
                new Category { CategoryId = 2, Name = "Toys", IsDeleted = false },
                new Category { CategoryId = 3, Name = "Tech", IsDeleted = false }
                );
        }
    }
}
