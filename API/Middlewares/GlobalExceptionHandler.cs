using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;

namespace Lab1.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = exception switch
            {
                FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ArgumentException => StatusCodes.Status400BadRequest,
                NullReferenceException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = exception.Message,
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true; 
        }
    }
}