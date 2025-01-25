namespace ShoppingApp.WebApi.Middlewares
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _isInMaintenance;

        public MaintenanceMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _isInMaintenance = bool.Parse(configuration["Maintenance:Enabled"] ?? "false");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_isInMaintenance)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\":\"API şu anda bakım modunda. Daha sonra tekrar deneyin.\"}");
                return;
            }

            await _next(context);
        }
    }
}
