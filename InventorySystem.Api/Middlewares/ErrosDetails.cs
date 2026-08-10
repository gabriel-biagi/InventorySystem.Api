using System.Text.Json;

namespace InventorySystem.Api.Middlewares;

public class ErrorDetails
{
    public int StatusCode { get; private set; }
    public string?  Message { get; private set; }
    public string? TraceId { get; private set; }


    public ErrorDetails(int statusCode, string? message, string? traceId)
    {
        StatusCode = statusCode;
        Message = message;
        TraceId = traceId;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}