namespace UltimateDotNetSkeleton.Domain.Configurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using UltimateDotNetSkeleton.Domain.Models;

	public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.HasIndex(rt => new { rt.UserId, rt.DeviceId })
				.HasDatabaseName("IX_RefreshTokens_UserId_DeviceId");
		}
	}
}