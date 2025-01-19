using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var request = httpContext.Request;
        var userId = httpContext.User.Identity.Name;  // Kullanıcı bilgisi

        // Log kaydını başlatıyoruz
        _logger.LogInformation($"User: {userId} | Request Path: {request.Path} | Method: {request.Method}");

        // Middleware zincirinde sonraki adımı çağırıyoruz
        await _next(httpContext);

        // İsteğin sonucunu loglayabiliriz
        _logger.LogInformation($"User: {userId} | Response Status: {httpContext.Response.StatusCode}");
    }
}
