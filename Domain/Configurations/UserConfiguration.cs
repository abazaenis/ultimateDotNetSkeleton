namespace UltimateDotNetSkeleton.Domain.Configurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using UltimateDotNetSkeleton.Domain.Models;

	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.Property(u => u.RegistrationType)
				.HasConversion<string>();

			builder.HasMany(u => u.RefreshTokens)
				.WithOne(rt => rt.User)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}