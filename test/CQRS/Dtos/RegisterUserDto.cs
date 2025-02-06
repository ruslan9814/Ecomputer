namespace test.CQRS.Dtos;

public sealed record RegisterUserDto(
    string Email,      
    string Password,   
    string Name,       
    string Address    
);

