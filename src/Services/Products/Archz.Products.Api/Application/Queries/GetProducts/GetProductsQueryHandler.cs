using MediatR;

namespace Archz.Products.Api.Application.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
