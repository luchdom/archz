using MediatR;
using Microsoft.Extensions.Logging;

namespace Archz.Application.Core.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling request {RequestName} ({@Request})", request.GetType().Name, request);
        var response = await next();
        _logger.LogInformation("Request {RequestName} handled with response: {@Response}", request.GetType().Name, response);

        return response;
    }
}
