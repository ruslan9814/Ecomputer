using Domain.Carts;
using Domain.Products;

public class CartItem : EntityBase
{
    public int Quantity { get; private set; }
    public int ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public int CartId { get; private set; }
    public Cart Cart { get; private set; } = null!;

    public CartItem() { }

    public CartItem(int id, int quantity, int productId, Product product, int cartId, Cart cart)
        : base(id)
    {
        if (quantity < 0)
            throw new ArgumentException("Количество не может быть отрицательным", nameof(quantity));
        if (product == null)
            throw new ArgumentNullException(nameof(product), "Продукт не может быть null");
        if (product.Id != productId)
            throw new ArgumentException("ProductId не совпадает с идентификатором объекта Product.");
        if (cart == null)
            throw new ArgumentNullException(nameof(cart), "Корзина не может быть null");

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

        if (Product == null || Product.Id != productId)
            return Result.Failure("Товар не найден или не соответствует идентификатору.");

        if (newQuantity == Quantity)
            return Result.Success();

        if (newQuantity > Quantity)
        {
            int quantityToAdd = newQuantity - Quantity;

            if (Product.Quantity < quantityToAdd)
                return Result.Failure($"Недостаточно товара на складе. Доступно: {Product.Quantity}");

            var decreaseResult = Product.DecreaseQuantity(quantityToAdd);
            if (decreaseResult.IsFailure)
                return decreaseResult;
        }
        else
        {
            int quantityToReturn = Quantity - newQuantity;

            var increaseResult = Product.IncreaseQuantity(quantityToReturn);
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

        if (Product == null)
            return Result.Failure("Продукт не задан.");

        if (Quantity < quantity)
            return Result.Failure("Недостаточное количество товара в корзине для уменьшения.");

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

        if (Product == null)
            return Result.Failure("Продукт не задан.");

        if (Product.Quantity < quantity)
            return Result.Failure($"Недостаточно товара на складе. Доступно: {Product.Quantity}");

        var decreaseResult = Product.DecreaseQuantity(quantity);
        if (decreaseResult.IsFailure)
            return decreaseResult;

        Quantity += quantity;

        return Result.Success();
    }
}
