namespace UltimateDotNetSkeleton.Infrastructure.Extensions
{
	using System.Net;

	using Microsoft.AspNetCore.Diagnostics;

	using Serilog;

	using UltimateDotNetSkeleton.Application.Exceptions.BadRequest;
	using UltimateDotNetSkeleton.Application.Exceptions.NotFound;
	using UltimateDotNetSkeleton.Infrastructure.Exceptions;

	public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError,
                        };

                        Log.Error(contextFeature.Error, "Something went wrong: {Error}", contextFeature.Error);

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                        }.ToString());
                    }
                });
            });
        }
    }
}
