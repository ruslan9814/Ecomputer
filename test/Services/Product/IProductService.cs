using Test.Endpoints.Products.Requests;
using Test.Endpoints.Products.Responses;

namespace Test.Services.Product;

public interface IProductService
{
    Task<ProductResponse> GetProductAsync(GetProductRequest getProductRequest);
    Task<ProductResponse> AddProductAsync(AddProductRequest productRequest);
    Task<bool> RemoveProductAsync(RemoveProductRequest removeProductRequest);
    Task<ProductResponse> UpdateProductAsync(UpdateProductRequest productRequest);
}
