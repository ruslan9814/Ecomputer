namespace Application.Dtos;

public sealed record FavoriteDto(
    int Id,                
    ICollection<ProductDto> ProductIds, 
    int UserId
);
