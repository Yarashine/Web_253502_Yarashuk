namespace Web_253502_Yarashuk.UI.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        // Перехватываем запрос
        var requestPath = context.Request.Path;
        var statusCode = context.Response.StatusCode;


        // Логируем только те запросы, которые возвращают код состояния, отличный от 2XX
        if (statusCode < 200 || statusCode >= 300)
        {
            var timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz");
            _logger.LogWarning($"{timestamp} [WAR] ---> request {requestPath} returns {statusCode}");

        }
    }
}
