using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

// HTTP isteklerini ve yanıtlarını loglamak için bir middleware
public class LoggingMiddleware
{
    private readonly RequestDelegate _next; // Sonraki middleware'e geçişi sağlar
    private readonly ILogger<LoggingMiddleware> _logger; // Loglama işlemleri için kullanılan araç

    // Middleware'in constructor'ı, bağımlılıkları alır
    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // HTTP isteklerini işlemek ve loglamak için kullanılan metod
    public async Task InvokeAsync(HttpContext httpContext)
    {
        // İstek süresini ölçmek için bir zamanlayıcı başlatılır
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var request = httpContext.Request; // Gelen HTTP isteğini alır
            var userId = httpContext.User.Identity?.IsAuthenticated == true
                ? httpContext.User.Identity.Name // Kimliği doğrulanmış kullanıcı adı
                : "Anonim"; // Anonim kullanıcı

            // İstek bilgilerini loglar (Kullanıcı, İstek Yolu ve HTTP Metodu)
            _logger.LogInformation($"User: {userId} | Request Path: {request.Path} | Method: {request.Method}");

            // İstek bir sonraki middleware'e iletilir
            await _next(httpContext);

            // Zamanlayıcı durdurulur
            stopwatch.Stop();

            // Yanıt bilgilerini loglar (Kullanıcı, HTTP Durum Kodu ve İstek Süresi)
            _logger.LogInformation($"User: {userId} | Response Status: {httpContext.Response.StatusCode} | Duration: {stopwatch.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            // Hata durumunda zamanlayıcı durdurulur
            stopwatch.Stop();

            // Hata durumunu loglar (Kullanıcı, İstek Yolu, Hata Mesajı ve Süre)
            _logger.LogError($"User: {httpContext.User.Identity?.Name ?? "Anonim"} | Path: {httpContext.Request.Path} | Error: {ex.Message} | Duration: {stopwatch.ElapsedMilliseconds} ms");

            // Hatanın diğer middleware'lere iletilmesi sağlanır
            throw;
        }
    }
}
