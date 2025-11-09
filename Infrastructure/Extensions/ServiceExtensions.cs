namespace UltimateDotNetSkeleton.Infrastructure.Extensions
{
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.RateLimiting;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    using Serilog;
    using Serilog.Events;

    using UltimateDotNetSkeleton.Application.ConfigurationModels;
    using UltimateDotNetSkeleton.Application.Mappings;
    using UltimateDotNetSkeleton.Application.Services.Manager;
    using UltimateDotNetSkeleton.Domain.Context;
    using UltimateDotNetSkeleton.Domain.Repositories.Manager;
    using UltimateDotNetSkeleton.Infrastructure.Services.BlobStorage;
    using UltimateDotNetSkeleton.Infrastructure.Services.EmailSender;

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
					RoleClaimType = "Role",
				};
			});
		}

		public static void ConfigureRateLimitingOptions(this IServiceCollection services)
		{
			services.AddRateLimiter(options =>
			{
				options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

				options.AddPolicy("EmailPolicy", httpContext =>
					RateLimitPartition.GetFixedWindowLimiter(
						partitionKey: httpContext.User.FindFirst("UserId")?.Value ?? "Anonymous",
						factory: _ => new FixedWindowRateLimiterOptions
						{
							PermitLimit = 5,
							Window = TimeSpan.FromHours(1),
						}));

				options.AddPolicy("ForgotPasswordPolicy", httpContext =>
					RateLimitPartition.GetFixedWindowLimiter(
						partitionKey: httpContext.User.FindFirst("UserId")?.Value ?? "Anonymous",
						factory: _ => new FixedWindowRateLimiterOptions
						{
							PermitLimit = 5,
							Window = TimeSpan.FromHours(1),
						}));
			});
		}

		// TODO: Limit this at the end of development
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

		public static void ConfigureBlobStorageHandler(this IServiceCollection services) =>
			services.AddScoped<IBlobStorageHandler, BlobStorageHandler>();

		public static void ConfigureRepositoryManager(this IServiceCollection services) =>
			services.AddScoped<IRepositoryManager, RepositoryManager>();

		public static void ConfigureServiceManager(this IServiceCollection services) =>
			services.AddScoped<IServiceManager, ServiceManager>();

		public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
			services.AddDbContext<RepositoryContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));

		public static void ConfigureEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var emailConfig = new EmailConfiguration();
			configuration.GetSection(EmailConfiguration.Section).Bind(emailConfig);
			services.AddSingleton(emailConfig);
		}

		public static void ConfigureAutoMapper(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(config =>
			{
				config.LicenseKey = configuration.GetSection("AutoMapper")["LicenseKey"];
				config.AddProfile<MappingProfile>();
			});
		}

		public static void ConfigureApiBehaviorOptions(this IServiceCollection services)
		{
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});
		}

		public static IMvcBuilder ConfigureControllers(this IServiceCollection services)
		{
			static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
			{
				using var serviceProvider = new ServiceCollection()
					.AddLogging()
					.AddMvc()
					.AddNewtonsoftJson()
					.Services
					.BuildServiceProvider();

				return serviceProvider
					.GetRequiredService<IOptions<MvcOptions>>()
					.Value
					.InputFormatters
					.OfType<NewtonsoftJsonPatchInputFormatter>()
					.First();
			}

			var jsonPatchFormatter = GetJsonPatchInputFormatter();

			return services.AddControllers(config =>
			{
				config.RespectBrowserAcceptHeader = true;
				config.ReturnHttpNotAcceptable = true;
				config.InputFormatters.Insert(0, jsonPatchFormatter);
			})
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			})
			.AddXmlDataContractSerializerFormatters();
		}

		public static void ConfigureSwagger(this IServiceCollection services) =>
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
				{
					Title = "Ultimate .NET Skeleton API",
					Version = "v1",
				});

				options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					In = Microsoft.OpenApi.Models.ParameterLocation.Header,
					Description = "Enter your valid JWT token.",
				});
				options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
				{
					{
						new Microsoft.OpenApi.Models.OpenApiSecurityScheme
						{
							Reference = new Microsoft.OpenApi.Models.OpenApiReference
							{
								Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
								Id = "Bearer",
							},
						},
						Array.Empty<string>()
					},
				});
			});

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
