namespace UltimateDotNetSkeleton.Domain.Context
{
    using Microsoft.EntityFrameworkCore;
    using UltimateDotNetSkeleton.Domain.Configurations;
    using UltimateDotNetSkeleton.Domain.Models;

    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Company>? Companies { get; set; }

        public DbSet<Employee>? Employees { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
	}
}
