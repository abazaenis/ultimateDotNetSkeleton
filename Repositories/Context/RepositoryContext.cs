namespace UltimateDotNetSkeleton.Repositories.Context
{
    using Microsoft.EntityFrameworkCore;

    using UltimateDotNetSkeleton.Models;

    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Company>? Companies { get; set; }

        public DbSet<Employee>? Employees { get; set; }
    }
}
