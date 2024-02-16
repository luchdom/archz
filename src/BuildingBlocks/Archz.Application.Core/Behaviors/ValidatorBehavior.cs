using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;


namespace Archz.Application.Core.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : ResultBase, new()
{
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure =>
                new Error(validationFailure.ErrorMessage)
                .WithMetadata(validationFailure.ErrorCode, validationFailure.ErrorMessage))
            .ToList();

        if (errors.Count == 0)
        {
            var response = await next();

            return response;
        }

        _logger.LogWarning("One or more validation failures have occurred in {RequestName} {@ValidationErrors} {@Request}",
                request.GetType().Name, errors, request);
        var result = new TResponse();
        errors.ForEach(result.Reasons.Add);
        return result;
    }
}