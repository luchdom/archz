using System.ComponentModel.DataAnnotations;

namespace Archz.Core.Validation;

public class AbstractValidatableObject : IValidatableObject
{
    protected IList<ValidationResult> _errors;
    public AbstractValidatableObject()
    {
        _errors = new List<ValidationResult>();
    }
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        CancellationTokenSource source = new();

        var task = ValidateAsync(validationContext, source.Token);

        task.Wait();

        return task.Result;
    }

    public virtual Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext, CancellationToken cancellation)
    {
        return Task.FromResult((IEnumerable<ValidationResult>)new List<ValidationResult>());
    }
}
