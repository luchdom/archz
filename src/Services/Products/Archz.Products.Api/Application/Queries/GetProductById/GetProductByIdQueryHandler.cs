using MediatR;

namespace Archz.Products.Api.Application.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
