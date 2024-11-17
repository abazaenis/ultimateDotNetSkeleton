namespace UltimateDotNetSkeleton.Repository
{
	using Microsoft.EntityFrameworkCore;

	using UltimateDotNetSkeleton.Models;
	using UltimateDotNetSkeleton.Repository.Configuration;

	public class RepositoryContext : DbContext
	{
		public RepositoryContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Company>? Companies { get; set; }

		public DbSet<Employee>? Employees { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new CompanyConfiguration());
			modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
		}
	}
}
