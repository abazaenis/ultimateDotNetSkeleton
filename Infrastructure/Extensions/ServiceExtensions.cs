namespace UltimateDotNetSkeleton.Infrastructure.Extensions
{
	using System.Text;
	using System.Threading.RateLimiting;
	using AspNetCoreRateLimit;

	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.RateLimiting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Options;
	using Microsoft.IdentityModel.Tokens;

	using UltimateDotNetSkeleton.Application.Services.Manager;
	using UltimateDotNetSkeleton.Domain.Context;
	using UltimateDotNetSkeleton.Domain.Models;
	using UltimateDotNetSkeleton.Domain.Repositories.Manager;
	using UltimateDotNetSkeleton.Infrastructure.Logger;

	public static class ServiceExtensions
    {
		public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtSettings = configuration.GetSection("JwtSettings");
			var secretKey = jwtSettings["secretKey"];

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
					ValidIssuer = jwtSettings["validIssuer"],
					ValidAudience = jwtSettings["validAudience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
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
							Window = TimeSpan.FromMinutes(10)
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
