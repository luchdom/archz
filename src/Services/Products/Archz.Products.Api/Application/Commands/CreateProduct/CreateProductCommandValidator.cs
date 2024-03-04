using FluentValidation;

namespace Archz.Products.Api.Application.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
    }
}
