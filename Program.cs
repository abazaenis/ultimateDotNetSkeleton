namespace UltimateDotNetSkeleton
{
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Options;

    using NLog;
    using UltimateDotNetSkeleton.Application.Extensions;
    using UltimateDotNetSkeleton.Infrastructure.Logger;
    using UltimateDotNetSkeleton.Presentation.ActionFilters;

    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Logger configuration
			var loggerConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "nlog.config");
			LogManager.Setup().LoadConfigurationFromFile(loggerConfigFilePath);

			NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
				new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
				.Services.BuildServiceProvider()
				.GetRequiredService<IOptions<MvcOptions>>()
				.Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();

			// Service configuration
			builder.Services.ConfigureCors();
			builder.Services.ConfigureLoggerService();
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

			var app = builder.Build();

			var logger = app.Services.GetRequiredService<ILoggerManager>();
			app.ConfigureExceptionHandler(logger);

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

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
