namespace UltimateDotNetSkeleton.Infrastructure.Services.DateTimeHelper
{
	public static class DateTimeHelper
	{
		public static DateTime GetUnspecifiedUtcNow() => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
	}
}
