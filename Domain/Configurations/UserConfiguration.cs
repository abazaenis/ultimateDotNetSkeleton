namespace UltimateDotNetSkeleton.Domain.Configurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using UltimateDotNetSkeleton.Domain.Models;

	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			ConfigureTable(builder);
			ConfigureKey(builder);
			ConfigureProperties(builder);
			ConfigureRelationships(builder);
		}

		private static void ConfigureTable(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");
		}

		private static void ConfigureKey(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(u => u.Id);
			builder.Property(u => u.Id)
				.HasColumnName("UserId")
				.ValueGeneratedOnAdd();
		}

		private static void ConfigureProperties(EntityTypeBuilder<User> builder)
		{
			builder.Property(u => u.FirstName)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(u => u.LastName)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(u => u.PhoneNumber)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(u => u.Email)
				.IsRequired()
				.HasMaxLength(254);

			builder.Property(u => u.ThirdPartyId)
				.HasMaxLength(255);

			builder.Property(u => u.RegistrationType)
				.IsRequired()
				.HasConversion<string>()
				.HasMaxLength(50);

			builder.Property(u => u.TempPasswordExpiry)
				.HasColumnType("timestamp without time zone");

			builder.Property(u => u.RegistrationDate)
				.IsRequired()
				.HasColumnType("timestamp without time zone");

			builder.Property(u => u.DeletedAt)
				.HasColumnType("timestamp without time zone");

			builder.Property(u => u.EmailNotifications)
				.HasDefaultValue(true);

			builder.Property(u => u.IsDeleted)
				.HasDefaultValue(false);
		}

		private static void ConfigureRelationships(EntityTypeBuilder<User> builder)
		{
			builder.HasMany(u => u.RefreshTokens)
				.WithOne(rt => rt.User)
				.HasForeignKey(rt => rt.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
