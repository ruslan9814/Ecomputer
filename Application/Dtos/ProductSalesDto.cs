
namespace Application.Dtos;

public sealed record ProductSalesDto
(
    int ProductId,
    string ProductName,
    int TotalQuantity,
    decimal TotalRevenue
);
