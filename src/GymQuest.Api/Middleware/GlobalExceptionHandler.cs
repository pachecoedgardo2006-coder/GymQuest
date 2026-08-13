using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GymQuest.Api.Middleware;

/// <summary>
/// Captura CUALQUIER excepción no manejada que llegue hasta aquí (desde
/// cualquier Controller) y la convierte en una respuesta HTTP consistente
/// en formato ProblemDetails (RFC 7807), en vez de un 500 genérico sin
/// contexto o -peor- una página HTML de error.
///
/// Se registra en Program.cs con:
///   builder.Services.AddExceptionHandler&lt;GlobalExceptionHandler&gt;();
///   builder.Services.AddProblemDetails();
/// y se activa en el pipeline con: app.UseExceptionHandler();
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, titulo) = MapearExcepcion(exception);

        _logger.LogError(exception, "Excepción no controlada: {Mensaje}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = titulo,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // true = "ya lo manejé, no dejes que se propague más"
    }

    private static (int StatusCode, string Titulo) MapearExcepcion(Exception exception) => exception switch
    {
        ArgumentException => ((int)HttpStatusCode.BadRequest, "Solicitud inválida"),
        InvalidOperationException => ((int)HttpStatusCode.BadRequest, "No se pudo completar la operación"),
        _ => ((int)HttpStatusCode.InternalServerError, "Ocurrió un error inesperado")
    };
}