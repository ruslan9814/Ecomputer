namespace test.Endpoints.Products.Requests;

public sealed record UpdateProductRequest(
    int Id,
    string? Name,
    decimal Price,
    string? Description,
    int Quantity,
    bool IsInStock);
