namespace UltimateDotNetSkeleton.Infrastructure.Extensions
{
	using System.Text;
	using System.Threading.RateLimiting;

	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.IdentityModel.Tokens;

	using Serilog;
	using Serilog.Events;

	using UltimateDotNetSkeleton.Application.ConfigurationModels;
	using UltimateDotNetSkeleton.Application.Services.Manager;
	using UltimateDotNetSkeleton.Domain.Context;
	using UltimateDotNetSkeleton.Domain.Models;
	using UltimateDotNetSkeleton.Domain.Repositories.Manager;
	using UltimateDotNetSkeleton.Infrastructure.Services.EmailSender;

	using IEmailSender = UltimateDotNetSkeleton.Infrastructure.Services.EmailSender.IEmailSender;

	public static class ServiceExtensions
    {
		public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtConfiguration = new JwtConfiguration();
			configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

			var secretKey = jwtConfiguration.SecretKey;

			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtConfiguration.ValidIssuer,
					ValidAudience = jwtConfiguration.ValidAudience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
				};
			});
		}

		public static void ConfigureRateLimitingOptions(this IServiceCollection services)
		{
			services.AddRateLimiter(options =>
			{
				options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

				options.AddPolicy("FixedTokenPolicy", httpContext =>
					RateLimitPartition.GetFixedWindowLimiter(
						partitionKey: httpContext.User.Identity?.Name?.ToString(),
						factory: _ => new FixedWindowRateLimiterOptions
						{
							PermitLimit = 10,
							Window = TimeSpan.FromMinutes(10),
						}));
			});
		}

		public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination"));
            });

		public static void ConfigureEmailSender(this IServiceCollection services) =>
			services.AddScoped<IEmailSender, EmailSender>();

		public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
        }

		public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

		public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

		public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<RepositoryContext>(opts => opts.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));

		public static void ConfigureEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var emailConfig = new EmailConfiguration();
			configuration.GetSection(EmailConfiguration.Section).Bind(emailConfig);
			services.AddSingleton(emailConfig);
		}

		public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
		{
			const string AzureBlobStorageConnection = "AzureBlobStorage";
			const string DotNetEnvironment = "ASPNETCORE_ENVIRONMENT";
			const string DevelopmentEnvironment = "Development";

			var environment = Environment.GetEnvironmentVariable(DotNetEnvironment) ?? DevelopmentEnvironment;

			var loggerConfig = new LoggerConfiguration()
				.MinimumLevel.Information()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("System", LogEventLevel.Warning)
				.Enrich.FromLogContext()
				.Enrich.WithProperty("Environment", environment)
				.WriteTo.Console();

			if (string.Equals(environment, DevelopmentEnvironment, StringComparison.OrdinalIgnoreCase))
			{
				loggerConfig.WriteTo.Debug(restrictedToMinimumLevel: LogEventLevel.Information);
			}
			else
			{
				var connectionString = configuration.GetConnectionString(AzureBlobStorageConnection);

				if (!string.IsNullOrEmpty(connectionString))
				{
					loggerConfig.WriteTo.AzureBlobStorage(
						connectionString: connectionString,
						restrictedToMinimumLevel: LogEventLevel.Information,
						storageContainerName: $"logs-{environment.ToLower()}",
						storageFileName: "{yyyy}/{MM}/log.txt",
						retainedBlobCountLimit: 2,
						blobSizeLimitBytes: 50 * 1024 * 1024);
				}
				else
				{
					Console.WriteLine($"Warning: Azure Blob Storage connection string not found for environment '{environment}'");
				}
			}

			var logger = loggerConfig.CreateLogger();
			Log.Logger = logger;

			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.AddSerilog(logger, dispose: true);
			});

			return services;
		}

		public static string AddTennoSignature()
		{
			var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
			var signaturePath = Path.Combine(
				webRootPath,
				"Signature",
				"TennoSignature.txt");

			var formattedTime = DateTime.Now.ToString("HH:mm:ss");
			var file = File.ReadAllText(signaturePath)
				.Replace("{{CURRENT_TIME}}", formattedTime);

			return file;
		}
	}
}
