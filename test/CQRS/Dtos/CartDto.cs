
namespace test.CQRS.Dtos;

public sealed record CartDto(
    int Id,
    decimal TotalPrice,
    ICollection<CartItemDto> CartItemsDto
    );
