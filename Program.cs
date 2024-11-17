namespace UltimateDotNetSkeleton
{
    using Microsoft.AspNetCore.HttpOverrides;
    using NLog;
    using UltimateDotNetSkeleton.Extensions;

    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Logger configuration
			var loggerConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "nlog.config");
			LogManager.Setup().LoadConfigurationFromFile(loggerConfigFilePath);

			// Service configuration
			builder.Services.ConfigureCors();
			builder.Services.ConfigureLoggerService();
			builder.Services.ConfigureRepositoryManager();
			builder.Services.ConfigureServiceManager();
			builder.Services.ConfigureSqlContext(builder.Configuration);
			builder.Services.AddControllers();

			var app = builder.Build();

			// Configure middleware pipeline
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.All,
			});

			app.UseCors("CorsPolicy");

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
