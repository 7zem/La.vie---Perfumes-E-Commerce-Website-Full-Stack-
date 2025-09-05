using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Perfumes.DAL.Exceptions;
using System.Net;
using System.Text.Json;

namespace Perfumes.API.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new
            {
                Message = exception.Message,
                StatusCode = HttpStatusCode.InternalServerError,
                Timestamp = DateTime.UtcNow
            };

            switch (exception)
            {
                case EntityNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = new
                    {
                        Message = exception.Message,
                        StatusCode = HttpStatusCode.NotFound,
                        Timestamp = DateTime.UtcNow
                    };
                    break;

                case DuplicateEntityException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse = new
                    {
                        Message = exception.Message,
                        StatusCode = HttpStatusCode.Conflict,
                        Timestamp = DateTime.UtcNow
                    };
                    break;

                case System.InvalidOperationException:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    errorResponse = new
                    {
                        Message = exception.Message,
                        StatusCode = HttpStatusCode.BadRequest,
                        Timestamp = DateTime.UtcNow
                    };
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(exception, "An unhandled exception occurred");
                    errorResponse = new
                    {
                        Message = "An internal server error occurred.",
                        StatusCode = HttpStatusCode.InternalServerError,
                        Timestamp = DateTime.UtcNow
                    };
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }
    }
} 