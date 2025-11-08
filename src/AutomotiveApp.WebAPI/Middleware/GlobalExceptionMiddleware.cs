using System.Net;
using System.Text.Json;
using AutomotiveApp.Shared.Exceptions;
using AutomotiveApp.Shared.Response;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Middleware
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest,
                    [.. ex.Errors.Select(e => e.ErrorMessage)]);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized, "User not authenticated or token invalid.");
            }
            catch (ForbiddenAccessException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.GetType().IsGenericType && ex.GetType().GetGenericTypeDefinition() == typeof(NotFoundException<>))
                {
                    await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound, ex.Message);
                }
                else
                {
                    await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, [ex.Message]);
                }
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode httpStatus, params string[] errors)
        {
            _logger.LogError("Exception: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatus;

            var response = new ApiResponse<string>
            {
                Success = false,
                StatusCode = httpStatus,
                Errors = errors
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}