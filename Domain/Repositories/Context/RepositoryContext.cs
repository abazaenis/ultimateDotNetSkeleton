namespace UltimateDotNetSkeleton.Domain.Repositories.Context
{
	using Microsoft.EntityFrameworkCore;

	using UltimateDotNetSkeleton.Domain.Models;
	using UltimateDotNetSkeleton.Domain.Repositories.Configuration;

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
			base.OnModelCreating(modelBuilder);

			// Apply the EmployeeConfiguration
			modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

			// You can apply other IEntityTypeConfiguration<> classes here as well
		}
	}
}
