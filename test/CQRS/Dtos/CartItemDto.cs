namespace test.CQRS.Dtos;
public record CartItemDto(
    int Id,
    int Quantity,
    ProductDto Product
    );
