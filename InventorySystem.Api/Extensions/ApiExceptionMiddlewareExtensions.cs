using System.Net;
using InventorySystem.Api.Middlewares;
using Microsoft.AspNetCore.Diagnostics;

namespace InventorySystem.Api.Extensions;

public static class ApiExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
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
                    await context.Response.WriteAsync(new ErrorDetails(context.Response.StatusCode,
                        contextFeature.Error.Message, contextFeature.Error.StackTrace).ToString());
                }
            });
        });
    }
}