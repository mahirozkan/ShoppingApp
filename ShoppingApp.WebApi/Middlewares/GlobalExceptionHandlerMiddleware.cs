using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

// Tüm uygulama genelinde istisna yönetimini sağlayan bir middleware sınıfı
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next; // Sonraki middleware'e ilerlemeyi sağlar
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger; // Loglama için kullanılır

    // Middleware'in constructor'ı, bağımlılıkları alır
    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // HTTP isteklerini işlemek için InvokeAsync metodu
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Bir sonraki middleware'i çalıştırır
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            // Bir istisna oluştuğunda hatayı loglar ve özel hata yanıtını işler
            _logger.LogError($"Bir hata oluştu: {ex}");
            await HandleExceptionAsync(httpContext, ex); // Hata yanıtı oluşturur
        }
    }

    // İstisna oluştuğunda hata yanıtını işleyecek özel metot
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Varsayılan olarak HTTP 500 (Internal Server Error) durum kodu
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "Sunucu tarafında bir hata oluştu."; // Genel hata mesajı
        var detail = exception.Message; // İstisna mesajı detayları

        // Farklı istisna türlerine göre durum kodu ve mesaj belirleme
        if (exception is UnauthorizedAccessException)
        {
            statusCode = (int)HttpStatusCode.Unauthorized; // Yetkisiz erişim durumunda 401
            message = "Yetkisiz erişim.";
        }
        else if (exception is KeyNotFoundException)
        {
            statusCode = (int)HttpStatusCode.NotFound; // Kaynak bulunamadığında 404
            message = "Kaynak bulunamadı.";
        }

        // Hata yanıtını JSON formatında oluşturur
        var errorResponse = new
        {
            statusCode, // HTTP durum kodu
            message, // Hata mesajı
            detail // Hata detayı
        };

        // HTTP yanıtının içeriğini ve durum kodunu ayarlar
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        // Hata yanıtını JSON formatında döner
        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}
