using System.Net;
using System.Text.Json;
using Booking.BLL.Exceptions;

namespace Booking.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        // RequestDelegate — это ссылка на следующее звено в конвейере (следующий Middleware или сам Контроллер)
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Этот метод вызывается автоматически при каждом HTTP-запросе
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Передаем запрос дальше по конвейеру (в сторону контроллера)
                await _next(context);
            }
            catch (Exception ex)
            {
                // Если где-то "глубже" (в сервисе или репозитории) упала ошибка,
                // мы перехватываем её здесь и обрабатываем в одном месте
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // По умолчанию - 500 ошибка
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Произошла непредвиденная ошибка на сервере";

            if (exception is ConflictException)
            {
                statusCode = HttpStatusCode.Conflict;   // Код 409
                message = exception.Message;
            }
            else if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;   // Код 404
                message = exception.Message;
            }
            else if (exception is ValidationException)
            {
                statusCode = HttpStatusCode.NotImplemented;   // Код 501
                message = exception.Message;
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;   // Код 404
                message = exception.Message;
            }

            context.Response.StatusCode = (int)statusCode;

            var responsePayload = new { error = message };
            var jsonResponse = JsonSerializer.Serialize(responsePayload);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
