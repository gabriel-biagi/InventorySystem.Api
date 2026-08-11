using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InventorySystem.Api.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;
    private Stopwatch? _stopwatch;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("[INÍCIO] Processando rota: {Route}", context.HttpContext.Request.Path);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch?.Stop();
        var elapsedMs = _stopwatch?.ElapsedMilliseconds ?? 0;

        _logger.LogInformation("[FIM] Rota: {Route} | Status: {Status} | Tempo: {Time}ms",
            context.HttpContext.Request.Path,
            context.HttpContext.Response.StatusCode,
            elapsedMs);
    }
}