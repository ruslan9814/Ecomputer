 
namespace Application.Dtos;

public sealed record ProductRatingDto(
    int ProductId,
    string ProductName,
    double AverageRating,
    int ReviewCount);

