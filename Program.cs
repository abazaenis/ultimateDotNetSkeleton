namespace UltimateDotNetSkeleton
{
	using Microsoft.AspNetCore.HttpOverrides;
	using ultimateDotNetSkeleton.Extensions;

	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Service configuration
			builder.Services.ConfigureCors();
			builder.Services.ConfigureIISIntegration();
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

			app.Use(async (context, next) =>
			{
				await context.Response.WriteAsync("Hello from the middleware component1\n");
				await next();
			});

			app.Use(async (context, next) =>
			{
				await context.Response.WriteAsync("Hello from the middleware component2\n");
				await next();
			});

			app.MapControllers();

			app.Run();
		}
	}
}
