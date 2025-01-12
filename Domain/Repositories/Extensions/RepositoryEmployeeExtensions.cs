namespace UltimateDotNetSkeleton.Domain.Repositories.Extensions
{
    using System.Linq.Dynamic.Core;
    using Microsoft.EntityFrameworkCore;
    using UltimateDotNetSkeleton.Domain.Models;
    using UltimateDotNetSkeleton.Domain.Repositories.Extensions.Utility;

    public static class RepositoryEmployeeExtensions
	{
		public static IQueryable<Employee> FilterByAge(this IQueryable<Employee> employees, uint minAge, uint maxAge)
		{
			return employees.Where(e => e.Age >= minAge && e.Age <= maxAge);
		}

		public static IQueryable<Employee> SearchByTrigram(this IQueryable<Employee> employees, string searchTerm)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
			{
				return employees;
			}

			var trimmedSearchTerm = searchTerm.Trim();
			const double similarityThreshold = 0.1;

			return employees
				.Where(e =>
					EF.Functions.TrigramsSimilarity(e.Name + " " + e.Position, trimmedSearchTerm)
						> similarityThreshold)
				.OrderByDescending(e =>
					EF.Functions.TrigramsSimilarity(e.Name + " " + e.Position, trimmedSearchTerm));
		}

		public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
		{
			if (string.IsNullOrWhiteSpace(orderByQueryString))
			{
				return employees.OrderBy(e => e.Name);
			}

			var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

			if (string.IsNullOrWhiteSpace(orderQuery))
			{
				return employees.OrderBy(e => e.Name);
			}

			return employees.OrderBy(orderQuery);
		}
	}
}
