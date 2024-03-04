using MediatR;

namespace Archz.Products.Api.Application.Queries.GetProducts;

public class GetProductsQuery: IRequest<IEnumerable<ProductDto>>
{
}
