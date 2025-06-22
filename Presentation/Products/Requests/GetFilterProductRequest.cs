

namespace Presentation.Products.Requests;

public sealed record GetFilterProductRequest
(
    string? Name,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool? IsInStock,
    int? CategoryId
);
