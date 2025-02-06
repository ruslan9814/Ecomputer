using test.Common;
using test.CQRS.Dtos;
using Test.Database.Repositories.Interfaces;

namespace test.CQRS.Carts.Queries;

public sealed record GetByUserIdCart(int UserId) : IRequest<Result<CartDto>>;

public sealed class GetByUserIdCartHandler(ICartRepository cartRepository) 
    : IRequestHandler<GetByUserIdCart, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    public async Task<Result<CartDto>> Handle(GetByUserIdCart request, CancellationToken cancellationToken)
    {
        var isCartExist = await _cartRepository.IsExistAsync(request.UserId);

        if (!isCartExist)
        {
            return (Result<CartDto>)Result.Failure("Корзина не найдена.");
        }


        var cart = await _cartRepository.GetAsync(request.UserId);

        ICollection<CartItemDto> cartItemDtos = [];

        foreach (var cartItem_ in cart.Items)
        {
            var cartItemDto = new CartItemDto(
                cartItem_.Id,  
                cartItem_.Quantity,  
                new ProductDto(   
                    cartItem_.ProductId,              
                    cartItem_.Product.Name,           
                    cartItem_.Product.Description,    
                    cartItem_.Product.Price,        
                    cartItem_.Product.IsInStock,      
                    cartItem_.Product.CreatedDate   
                )
            );

          
            cartItemDtos.Add(cartItemDto);
        }

       
        var cartDto = new CartDto(
            request.UserId,           
            cart.Items.Count,         
            cart.TotalPrice,          
            cartItemDtos             
        );

        return Result<CartDto>.Success(cartDto);
    }
}
