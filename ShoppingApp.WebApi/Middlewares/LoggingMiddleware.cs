using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
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
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var request = httpContext.Request;
            var userId = httpContext.User.Identity?.IsAuthenticated == true
                ? httpContext.User.Identity.Name
                : "Anonim";

            _logger.LogInformation($"User: {userId} | Request Path: {request.Path} | Method: {request.Method}");

            await _next(httpContext);

            stopwatch.Stop();

            _logger.LogInformation($"User: {userId} | Response Status: {httpContext.Response.StatusCode} | Duration: {stopwatch.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError($"User: {httpContext.User.Identity?.Name ?? "Anonim"} | Path: {httpContext.Request.Path} | Error: {ex.Message} | Duration: {stopwatch.ElapsedMilliseconds} ms");
            throw;
        }
    }
}
