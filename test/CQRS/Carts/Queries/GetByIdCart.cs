using test.Common;
using test.CQRS.Dtos;
using Test.Database.Repositories.Interfaces;
using Test.Models;

namespace test.CQRS.Carts.Queries
{
    public sealed record GetByIdCart(int UserId) : IRequest<Result<CartDto>>;

    public sealed class GetByIdCartHandler(ICartRepository cartRepository, ICartItemRepository cartItemRepository) : IRequestHandler<GetByIdCart, Result<CartDto>>
    {
        private readonly ICartRepository _cartRepository = cartRepository;
        private readonly ICartItemRepository _cartItemRepository = cartItemRepository;

        public async Task<Result<CartDto>> Handle(GetByIdCart request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetAsync(request.UserId);

            if (cart is null)
            {
                return (Result<CartDto>)Result.Failure("Корзина не найдена.");
            }

            var cartItem = await _cartItemRepository.GetAsync(cart.Id);

            if (cartItem is null)
            {
                return (Result<CartDto>)Result.Failure("Нет товаров в корзине.");
            }

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
                cart.TotalSum,          
                cartItemDtos             
            );


            return (Result<CartDto>)Result.Success;
        }
    }
}
