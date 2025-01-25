using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ShoppingApp.WebApi.Filters
{
    // Saat bazlı yetkilendirme kontrolü yapan bir filtre
    public class TimeBasedAuthorizationFilter : IActionFilter
    {
        // Erişim saat aralığı tanımları
        private readonly int _startHour = 7; // Başlangıç saati (07:00 UTC)
        private readonly int _endHour = 24; // Bitiş saati (24:00 UTC)

        // Action çalıştırılmadan önce çağrılır
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Geçerli saati UTC formatında alır
            var currentHour = DateTime.UtcNow.Hour;

            // Eğer mevcut saat izin verilen aralıkta değilse, erişimi engelle
            if (currentHour < _startHour || currentHour >= _endHour)
            {
                // ForbidResult ile erişim reddedilir (HTTP 403)
                context.Result = new ForbidResult();
            }
        }

        // Action çalıştırıldıktan sonra çağrılır (Bu örnekte kullanılmıyor)
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Bu metot şimdilik boş, ancak loglama gibi işlemler için kullanılabilir
        }
    }
}
