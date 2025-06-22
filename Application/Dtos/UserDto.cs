namespace Application.Dtos;
public sealed record UserDto(
    int Id,
    string Name,
    string Email,
    string Address,
    string? ImageUrl
);

