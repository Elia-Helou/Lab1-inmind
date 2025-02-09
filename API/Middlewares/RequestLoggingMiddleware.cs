using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Lab1.API.Middlewares
{
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
            var request = context.Request;
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            _logger.LogInformation($"[{timestamp}] Request started: {request.Method} {request.Path}{request.QueryString}");

            foreach (var header in request.Headers)
            {
                _logger.LogInformation($"[{timestamp}] Header: {header.Key}: {header.Value}");
            }

            if (request.ContentLength > 0 && request.Body.CanRead)
            {
                var originalRequestBody = request.Body;
                using (var reader = new StreamReader(request.Body, Encoding.UTF8))
                {
                    var body = await reader.ReadToEndAsync();
                    _logger.LogInformation($"[{timestamp}] Request Body: {body}");

                    request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
                }
            }

            var originalBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await _next(context);

                var responseStatusCode = context.Response.StatusCode;
                _logger.LogInformation($"[{timestamp}] Response status: {responseStatusCode}");

                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}
