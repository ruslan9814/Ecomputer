
namespace test.CQRS.Dtos;

public sealed record CartDto(
    int Id,
    int Quantity,
    decimal TotalPrice,
    ICollection<CartItemDto> CartItemDtos
    );
