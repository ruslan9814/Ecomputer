using Domain.Categories;

namespace Application.Endpoints.Products.Requests;

public sealed record GetFilterProductRequest
(
    string? Name,
    decimal MinPrice,
    decimal MaxPrice,
    bool IsInStock,
    Category? Category 
);