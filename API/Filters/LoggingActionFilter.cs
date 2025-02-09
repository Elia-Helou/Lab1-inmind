using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Lab2.API.Filters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            var parameters = JsonSerializer.Serialize(context.ActionArguments);

            _logger.LogInformation($"[START] Executing action: {actionName}");
            _logger.LogInformation($"[PARAMETERS] {parameters}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            var statusCode = context.HttpContext.Response.StatusCode;

            var responseBody = string.Empty;
            if (context.Result is ObjectResult objectResult)
            {
                responseBody = JsonSerializer.Serialize(objectResult.Value);
            }
            
            _logger.LogInformation($"[END] Action executed: {actionName}");
            _logger.LogInformation($"[RESPONSE] Status Code: {statusCode}");
            _logger.LogInformation($"[RESPONSE BODY] {responseBody}");
        }
    }
}
