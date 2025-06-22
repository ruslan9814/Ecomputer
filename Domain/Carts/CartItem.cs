using Domain.Carts;
using Domain.Products;

public class CartItem : EntityBase
{
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int CartId { get; set; }
    public Cart? Cart { get; set; }

    public CartItem() { }

    public CartItem(int id, int quantity, int productId, Product product, int cartId, Cart cart) 
        : base(id)
    {
        Quantity = quantity;
        ProductId = productId;
        Product = product;
        CartId = cartId;
        Cart = cart;
    }

    public Result UpdateQuantity(int productId, int newQuantity)
    {
        if (newQuantity < 0)
            return Result.Failure("Количество не может быть отрицательным.");

        if (productId <= 0)
            return Result.Failure("Неверный идентификатор товара.");

        if (Product is null || Product.Id != productId)
            return Result.Failure("Товар не найден.");

        if (newQuantity == Quantity)
            return Result.Success();

        if (newQuantity > Quantity)
        {
            int quantityToIncrease = newQuantity - Quantity;

            if (Product.Quantity < quantityToIncrease)
                return Result.Failure($"Недостаточно товара на складе. Доступно: {Product.Quantity}");

            var decreaseResult = Product.DecreaseQuantity(quantityToIncrease);
            if (decreaseResult.IsFailure)
                return decreaseResult;
        }
        else
        {
            int quantityToDecrease = Quantity - newQuantity;
            var increaseResult = Product.IncreaseQuantity(quantityToDecrease);
            if (increaseResult.IsFailure)
                return increaseResult;
        }

        Quantity = newQuantity;
        return Result.Success();
    }

    public Result DecreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure("Количество для уменьшения должно быть больше нуля.");

        if (Quantity - quantity < 0)
            return Result.Failure("Не хватает количества товара в корзине.");

        Quantity -= quantity;

        var increaseResult = Product.IncreaseQuantity(quantity);
        if (increaseResult.IsFailure)
            return increaseResult;

        return Result.Success();
    }

    public Result IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure("Количество для увеличения должно быть больше нуля.");

        if (Product.Quantity < quantity)
            return Result.Failure("Недостаточно товара на складе для увеличения количества.");

        var decreaseResult = Product.DecreaseQuantity(quantity);
        if (decreaseResult.IsFailure)
            return decreaseResult;

        Quantity += quantity;

        return Result.Success();
    }
}
