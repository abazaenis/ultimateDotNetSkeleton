using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UltimateDotNetSkeleton.Domain.Models;
using NpgsqlTypes;

namespace UltimateDotNetSkeleton.Domain.Repositories.Configuration
{
	public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{
			// Example constraints for name, position, etc.
			builder.Property(e => e.Name).IsRequired().HasMaxLength(30);
			builder.Property(e => e.Position).IsRequired().HasMaxLength(20);

			// 1) Shadow property of type NpgsqlTsVector (not string!)
			builder
				.Property<NpgsqlTsVector>("SearchVector")
				.HasColumnType("tsvector")
				.HasComputedColumnSql(
					@"to_tsvector('english', coalesce(""Name"", '') || ' ' || coalesce(""Position"", ''))",
					stored: true
				);

			// 2) Create a GIN index on that tsvector column
			builder
				.HasIndex("SearchVector")
				.HasMethod("GIN")
				.HasDatabaseName("IDX_Employees_SearchVector");
		}
	}
}
