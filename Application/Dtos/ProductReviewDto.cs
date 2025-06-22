

namespace Application.Dtos;

public record ProductReviewDto(
    int Id,
    int ProductId,
    int UserId,
    string UserName,
    string ProductName,
    string ProductDescription,
    string ReviewText,
    float Rating,
    DateTime CreatedAt
);


