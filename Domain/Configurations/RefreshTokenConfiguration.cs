namespace UltimateDotNetSkeleton.Domain.Configurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using UltimateDotNetSkeleton.Domain.Models;

	public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			ConfigureTable(builder);
			ConfigureKey(builder);
			ConfigureProperties(builder);
			ConfigureRelationships(builder);
			ConfigureIndexes(builder);
		}

		private static void ConfigureTable(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.ToTable("RefreshTokens");
		}

		private static void ConfigureKey(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.HasKey(rt => rt.Id);
			builder.Property(rt => rt.Id)
				.HasColumnName("RefreshTokenId")
				.ValueGeneratedOnAdd();
		}

		private static void ConfigureProperties(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.Property(rt => rt.Token)
				.IsRequired()
				.HasMaxLength(500);

			builder.Property(rt => rt.ExpirationDate)
				.IsRequired()
				.HasColumnType("timestamp without time zone");

			builder.Property(rt => rt.UserId)
				.IsRequired();

			builder.Property(rt => rt.DeviceId)
				.IsRequired()
				.HasMaxLength(255);
		}

		private static void ConfigureRelationships(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.HasOne(rt => rt.User)
				.WithMany(u => u.RefreshTokens)
				.HasForeignKey(rt => rt.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}

		private static void ConfigureIndexes(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.HasIndex(rt => new { rt.UserId, rt.DeviceId })
				.HasDatabaseName("IX_RefreshTokens_UserId_DeviceId");
		}
	}
}