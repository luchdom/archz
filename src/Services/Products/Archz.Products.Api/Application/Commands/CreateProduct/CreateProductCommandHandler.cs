using Archz.Products.Api.Domain.AggregateModels.ProductAggregates;
using Archz.SharedKernel.SeedWork;
using FluentResults;
using MediatR;

namespace Archz.Products.Api.Application.Commands.CreateProduct;

public class CreateProductCommandHandler(
    ILogger<CreateProductCommandHandler> logger,
    IProductRepository productRepository,
    IProductService productService,
    IUnitOfWork unitOfWork
        ) : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await productService.CreateProductAsync(request.Name, request.Price, cancellationToken);

        if (result.IsFailed)
            return result.ToResult<ProductDto>(null);

        await productRepository.InsertAsync(result.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        ProductDto productDto = result.Value;
        return Result.Ok(productDto);
    }
}
