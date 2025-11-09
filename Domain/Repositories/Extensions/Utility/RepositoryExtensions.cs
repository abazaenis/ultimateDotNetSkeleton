namespace UltimateDotNetSkeleton.Domain.Repositories.Extensions.Utility
{
	public static class RepositoryExtensions
	{
		public static IQueryable<T> ApplyIncludes<T>(
			this IQueryable<T> query,
			params Func<IQueryable<T>, IQueryable<T>>[] includes)
			where T : class
		{
			if (includes == null || includes.Length == 0)
				return query;

			return includes.Aggregate(query, (current, include) => include(current));
		}
	}
}
