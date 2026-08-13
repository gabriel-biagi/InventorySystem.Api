using System.Net;
using InventorySystem.Api.Middlewares;
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
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    if (env.IsDevelopment())
                    {
                        await context.Response.WriteAsync(new ErrorDetails(context.Response.StatusCode,
                            contextFeature.Error.Message, context.TraceIdentifier, contextFeature.Error.StackTrace).ToString());
                    }
                    else
                    {
                        await context.Response.WriteAsync(new ErrorDetails(context.Response.StatusCode,
                            contextFeature.Error.Message, context.TraceIdentifier, null).ToString());
                    }
                }
            });
        });
    }
}