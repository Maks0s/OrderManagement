using FluentValidation;

namespace OrderService.Application.Orders.CQRS.Commands.Add
{
    public class AddOrderCommandValidator
        : AbstractValidator<AddOrderCommand>
    {
        public AddOrderCommandValidator()
        {               
            RuleFor(adc => adc.ProductQuantity)
                .ExclusiveBetween(0, 10000);
        }
    }
}