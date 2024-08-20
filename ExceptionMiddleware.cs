using System.Net;
using Quiz_API.Exceptions;

namespace AuthenticationApp
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(httpContext, ex);
      }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      _logger.LogError(exception, "Unexpected error in your code");

      HttpStatusCode statusCode;
      string message = exception.Message;

      switch (exception)
      {
        case HttpRequestException _:
          statusCode = HttpStatusCode.BadRequest;
          if (context.Response.StatusCode == (int)HttpStatusCode.BadGateway)
          {
            statusCode = HttpStatusCode.BadGateway;
          }
          else if (context.Response.StatusCode == (int)HttpStatusCode.ServiceUnavailable)
          {
            statusCode = HttpStatusCode.ServiceUnavailable;
          }
          break;

        case UnauthorizedAccessException _:
          statusCode = HttpStatusCode.Unauthorized;
          if (string.IsNullOrWhiteSpace(message))
          {
            message = "Unauthorized access.";
          }
          break;

        case ForbiddenAccessException _:
          statusCode = HttpStatusCode.Forbidden;
          if (string.IsNullOrWhiteSpace(message))
          {
            message = "Access forbidden.";
          }
          break;

        case KeyNotFoundException _:
        case FileNotFoundException _:
        case NotFoundException _:
          statusCode = HttpStatusCode.NotFound;
          if (string.IsNullOrWhiteSpace(message))
          {
            message = "Resource not found.";
          }
          break;

        case NotSupportedException _:
          statusCode = HttpStatusCode.MethodNotAllowed;
          if (string.IsNullOrWhiteSpace(message))
          {
            message = "Method not allowed.";
          }
          break;

        case InvalidOperationException _:
        case ConflictException _:
          statusCode = HttpStatusCode.Conflict;
          if (string.IsNullOrWhiteSpace(message))
          {
            message = "Operation caused a conflict.";
          }
          break;

        case TimeoutException _:
          statusCode = HttpStatusCode.GatewayTimeout;
          if (string.IsNullOrWhiteSpace(message))
          {
            message = "The request timed out.";
          }
          break;

        default:
          statusCode = HttpStatusCode.InternalServerError;
          if (string.IsNullOrWhiteSpace(message))
          {
            message = "Internal server error.";
          }
          break;
      }

      var response = new
      {
        statusCode = (int)statusCode,
        message
      };

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)statusCode;

      return context.Response.WriteAsJsonAsync(response);
    }
  }

  public static class ExceptionMiddlewareExtensions
  {
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ExceptionMiddleware>();
    }
  }
}
