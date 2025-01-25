namespace ShoppingApp.WebApi.Middlewares
{
    // Uygulamanın bakım modunda olup olmadığını kontrol eden middleware
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next; // Sonraki middleware'e ilerlemeyi sağlar
        private readonly bool _isInMaintenance; // Bakım modunun aktif olup olmadığını tutar

        // Constructor: RequestDelegate ve IConfiguration bağımlılıklarını alır
        public MaintenanceMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            // Konfigürasyondan bakım modunun aktif olup olmadığını okur
            _isInMaintenance = bool.Parse(configuration["Maintenance:Enabled"] ?? "false");
        }

        // HTTP isteklerini işlemek için InvokeAsync metodu
        public async Task InvokeAsync(HttpContext context)
        {
            // Eğer bakım modu aktifse, isteği engelle ve 503 yanıtını döndür
            if (_isInMaintenance)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable; // 503 durum kodu
                context.Response.ContentType = "application/json"; // Yanıt içeriğini JSON olarak ayarlar

                // Bakım modu mesajını JSON formatında döndür
                await context.Response.WriteAsync("{\"message\":\"API şu anda bakım modunda. Daha sonra tekrar deneyin.\"}");
                return; // İstek işlenmeden sona erer
            }

            // Eğer bakım modu aktif değilse, isteği bir sonraki middleware'e iletir
            await _next(context);
        }
    }
}
