using Test.Database.Repositories.Interfaces;
using Test.Endpoints.Products.Responses;
using Test.Services.Product;


namespace Test.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<ProductResponse> GetProductAsync(GetProductRequest getProductRequest)
    {
        if (getProductRequest is null || getProductRequest.Id <= 0)
        {
            throw new ArgumentException("Invalid request: Product ID must be greater than 0.");
        }

        var product = await _productRepository.GetAsync(getProductRequest.Id);
        return product is null ? null : MapToProductResponse(product);
    }

    public async Task<ProductResponse> AddProductAsync(AddProductRequest productRequest)
    {
        if (string.IsNullOrEmpty(productRequest.Name) || productRequest.Price <= 0 || productRequest.Quantity < 0)
        {
            return null;
        }

        var product = new Product
        {
            Name = productRequest.Name,
            Price = productRequest.Price,
            Description = productRequest.Description,
            Quantity = productRequest.Quantity,
            IsInStock = productRequest.Quantity > 0,
            CreatedDate = DateTime.UtcNow
        };

        await _productRepository.AddAsync(product);

        return MapToProductResponse(product);
    }

    public async Task<bool> RemoveProductAsync(RemoveProductRequest removeProductRequest)
    {
        var product = await _productRepository.GetAsync(removeProductRequest.Id);
        if (product is null)
        {
            return false;
        }
        await _productRepository.DeleteAsync(removeProductRequest.Id);
        return true;
    }


    public async Task<ProductResponse> UpdateProductAsync(UpdateProductRequest updateProductRequest)
    {
        if (updateProductRequest.Id <= 0 || string.IsNullOrEmpty(updateProductRequest.Name))
        {
            return null;
        }

        var product = await _productRepository.GetAsync(updateProductRequest.Id);
        if (product is null)
        {
            return null;
        }

        product.Name = updateProductRequest.Name;
        product.Price = updateProductRequest.Price ?? product.Price;
        product.Description = updateProductRequest.Description ?? product.Description;
        product.Quantity = updateProductRequest.Quantity ?? product.Quantity;
        product.IsInStock = product.Quantity > 0;

        await _productRepository.UpdateAsync(product);

        return MapToProductResponse(product);
    }

    private static ProductResponse MapToProductResponse(Product product)
    {
        return new ProductResponse
        (
            product.Id,
            product.Name,
            product.Price,
            product.Description,
            product.Quantity,
            product.Quantity > 0,
            product.CreatedDate
        );
    }
}
