using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ShoppingApp.WebApi.Filters
{
    public class TimeBasedAuthorizationFilter : IActionFilter
    {
        private readonly int _startHour = 7;
        private readonly int _endHour = 24;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var currentHour = DateTime.UtcNow.Hour;

            if (currentHour < _startHour || currentHour >= _endHour)
            {
                context.Result = new ForbidResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
