using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using netech.Core.Exceptions;

namespace netech.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Ocorreu um erro não tratado.");

            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            if (exception is BusinessRuleException businessEx)
            {
                // Erros de Domínio (ex: Regra violada) -> 400 Bad Request
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Erro de Regra de Negócio";
                problemDetails.Detail = businessEx.Message;
                problemDetails.Status = StatusCodes.Status400BadRequest;
            }
            else
            {
                // Erros de Infraestrutura/Crash -> 500 Internal Server Error
                // Nota: Não expomos a mensagem da exceção (Stack Trace) por segurança ESG.
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Erro Interno do Servidor";
                problemDetails.Detail = "Ocorreu um erro inesperado. Contate o suporte.";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}