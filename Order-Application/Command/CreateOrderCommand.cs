using AutoMapper;
using FluentValidation;
using MediatR;
using Order_Domain.Domain;
using Order_Persistence;

namespace Order_Application.Command
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public string? CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string? ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);
            order.Id = Guid.NewGuid();
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.OrdersRepository.AddAsync(order);
            await _unitOfWork.CommitAsync(cancellationToken);
            return order.Id;
        }
    }
    public class CreateOrderCommandHandlerValidation : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandHandlerValidation()
        {
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.ShippingAddress).NotEmpty();
            RuleFor(x => x.CustomerEmail).NotEmpty();
            RuleFor(x => x.TotalAmount).GreaterThan(10);
        }
    }
}