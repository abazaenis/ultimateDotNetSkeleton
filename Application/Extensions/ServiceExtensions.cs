namespace UltimateDotNetSkeleton.Application.Extensions
{
	using Microsoft.EntityFrameworkCore;

	using UltimateDotNetSkeleton.Application.Services.Manager;
	using UltimateDotNetSkeleton.Domain.Repositories.Context;
	using UltimateDotNetSkeleton.Domain.Repositories.Manager;
	using UltimateDotNetSkeleton.Infrastructure.Logger;

	public static class ServiceExtensions
	{
		public static void ConfigureCors(this IServiceCollection services) =>
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy", builder =>
					builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());
			});

		public static void ConfigureLoggerService(this IServiceCollection services) =>
			services.AddSingleton<ILoggerManager, LoggerManager>();

		public static void ConfigureRepositoryManager(this IServiceCollection services) =>
			services.AddScoped<IRepositoryManager, RepositoryManager>();

		public static void ConfigureServiceManager(this IServiceCollection services) =>
			services.AddScoped<IServiceManager, ServiceManager>();

		public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
			services.AddDbContext<RepositoryContext>(opts => opts.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
	}
}
