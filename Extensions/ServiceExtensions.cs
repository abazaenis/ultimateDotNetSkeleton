namespace UltimateDotNetSkeleton.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using UltimateDotNetSkeleton.Repositories.Contracts;
    using UltimateDotNetSkeleton.Repositories.Implementations.Manager;
    using UltimateDotNetSkeleton.Repository.Context;
    using UltimateDotNetSkeleton.Services.Contracts;
    using UltimateDotNetSkeleton.Services.Implementations;
    using UltimateDotNetSkeleton.Utilities.Logger;

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
