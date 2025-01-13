namespace UltimateDotNetSkeleton.Domain.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NpgsqlTypes;
    using UltimateDotNetSkeleton.Domain.Models;

    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Name).IsRequired().HasMaxLength(30);
            builder.Property(e => e.Position).IsRequired().HasMaxLength(20);

            builder
                .Property<NpgsqlTsVector>("SearchVector")
                .HasColumnType("tsvector")
                .HasComputedColumnSql(
                    @"to_tsvector('english', coalesce(""Name"", '') || ' ' || coalesce(""Position"", ''))",
                    stored: true
                );

            builder
                .HasIndex("SearchVector")
                .HasMethod("GIN")
                .HasDatabaseName("IDX_Employees_SearchVector");
        }
    }
}
