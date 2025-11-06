namespace UltimateDotNetSkeleton
{
	using Microsoft.AspNetCore.HttpOverrides;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Formatters;
	using Microsoft.Extensions.Options;

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

			NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
				new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
				.Services.BuildServiceProvider()
				.GetRequiredService<IOptions<MvcOptions>>()
				.Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();

			// Service configuration
			builder.Services.ConfigureCors();
			builder.Services.ConfigureRepositoryManager();
			builder.Services.ConfigureServiceManager();
			builder.Services.ConfigureSqlContext(builder.Configuration);
			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});
			builder.Services.AddScoped<ValidationFilterAttribute>();
			builder.Services.AddControllers(config =>
			{
				config.RespectBrowserAcceptHeader = true;
				config.ReturnHttpNotAcceptable = true;
				config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
			}).AddXmlDataContractSerializerFormatters();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddAutoMapper(typeof(Program));
			builder.Services.AddAuthentication();
			builder.Services.AddMemoryCache();
			builder.Services.ConfigureRateLimitingOptions();
			builder.Services.AddHttpContextAccessor();
			builder.Services.ConfigureIdentity();
			builder.Services.ConfigureJWT(builder.Configuration);

			var app = builder.Build();

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
