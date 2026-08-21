using System.Net;
using InventorySystem.Api.Middlewares;
using InventorySystem.Domain.Exception;
using Microsoft.AspNetCore.Diagnostics;

namespace InventorySystem.Api.Extensions;

public static class ApiExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var exception = contextFeature.Error;
                    
                    context.Response.StatusCode = exception switch
                    {
                        NotFoundException => (int)HttpStatusCode.NotFound,
                        BusinessException => (int)HttpStatusCode.BadRequest,
                        _ => (int)HttpStatusCode.InternalServerError
                    };

                    var stackTrace = env.IsDevelopment() ? exception.StackTrace : null;
                    var errorResponse = new ErrorDetails(
                        context.Response.StatusCode,
                        exception.Message,
                        context.TraceIdentifier,
                        stackTrace
                    );

                    await context.Response.WriteAsync(errorResponse.ToString());
                }
            });
        });
    }
}