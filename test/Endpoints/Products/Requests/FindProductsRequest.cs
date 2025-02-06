namespace test.Endpoints.Products.Requests;

// TODO: Change logic
public sealed record FindProductsRequest(
    int? Id,
    string? Name,
    string? Category,
    decimal? MinPrice,
    decimal? MaxPrice,
    DateTime? CreatedAfter);
