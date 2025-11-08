namespace UltimateDotNetSkeleton.Infrastructure.Extensions
{
	using System;
	using System.Security.Claims;

	public static class ClaimsPrincipalExtensions
	{
		public static Guid GetUserId(this ClaimsPrincipal user)
		{
			var claim = user.FindFirst("UserId");

			if (claim is null || !Guid.TryParse(claim.Value, out var userId))
				throw new UnauthorizedAccessException("Korisnički ID nije pronađen u tokenu.");

			return userId;
		}
	}
}