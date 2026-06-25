using Microsoft.AspNetCore.Diagnostics;

namespace backend.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Необработанное исключение: {Message}", exception.Message);

        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(
            new { error = "Внутренняя ошибка сервера." },
            cancellationToken);

        return true;
    }
}