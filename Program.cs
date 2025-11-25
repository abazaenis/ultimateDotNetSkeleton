namespace UltimateDotNetSkeleton
{
	using Microsoft.AspNetCore.HttpOverrides;
	using Microsoft.Extensions.DependencyInjection;
	using Serilog;
	using UltimateDotNetSkeleton.Infrastructure.Extensions;
	using UltimateDotNetSkeleton.Presentation.ActionFilters;

	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Logger configuration
			builder.Services.ConfigureLogging(builder.Configuration);
			builder.Host.UseSerilog();

			// Service configuration
			builder.Services.ConfigureCors();
			builder.Services.ConfigureEmailSender();
			builder.Services.ConfigureBlobStorageHandler();
			builder.Services.ConfigureRepositoryManager();
			builder.Services.ConfigureServiceManager();
			builder.Services.ConfigureSqlContext(builder.Configuration);
			builder.Services.ConfigureEmailConfiguration(builder.Configuration);
			builder.Services.ConfigureApiBehaviorOptions();
			builder.Services.AddScoped<ValidationFilterAttribute>();
			builder.Services.ConfigureControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.ConfigureSwagger();
			builder.Services.ConfigureAutoMapper(builder.Configuration);

			builder.Services.AddAuthentication();
			builder.Services.ConfigureRateLimitingOptions();
			builder.Services.AddHttpContextAccessor();
			builder.Services.ConfigureJWT(builder.Configuration);
			builder.Services.ConfigureCompression();

			var app = builder.Build();
			app.MapGet("/", async context => await context.Response.WriteAsync(ServiceExtensions.AddTennoSignature()));

			app.ConfigureExceptionHandler();

			// Configure middleware pipeline
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
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

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseRateLimiter();

			app.MapControllers();

			app.Run();
		}
	}
}
