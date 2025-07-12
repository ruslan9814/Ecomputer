using Domain.Orders;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;
using Infrastrcture.Email; 

namespace Application.OrderItems.Command;

public sealed record UpdateOrderStatusCommand(int OrderId, OrderStatus NewStatus)
    : IRequest<Result>;

internal sealed class UpdateOrderStatusCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IEmailSenderService emailSender  
) : IRequestHandler<UpdateOrderStatusCommand, Result>
{
    private readonly IOrderRepository orderRepository = orderRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IEmailSenderService emailSender = emailSender;

    public async Task<Result> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.Get(request.OrderId);
        if (order is null)
        {
            return Result.Failure("Заказ не найден.");
        }

        var result = order.ChangeStatus(request.NewStatus);
        if (result.IsFailure)
        {
            return result;
        }

        await orderRepository.UpdateAsync(order);
        await unitOfWork.Commit();
         
        if (request.NewStatus == OrderStatus.Delivered)
        {
            var user = order.User;  
            if (user is not null && !string.IsNullOrWhiteSpace(user.Email))
            {
                var subject = "Ваш заказ доставлен в пункт выдачи";
                var htmlMessage = $"""
                    <h3>Здравствуйте, {user.Name ?? "пользователь"}!</h3>
                    <p>Ваш заказ №{order.Id} доставлен в пункт выдачи.</p>
                    <p>Вы можете его забрать в часы работы. Спасибо за покупку!</p>
                """;

                await emailSender.SendEmailAsync(user.Email, subject, htmlMessage);
            }
        }

        return Result.Success();
    }
}
