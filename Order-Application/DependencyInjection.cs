using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Order_Application.Command;

namespace Order_Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateOrderCommand>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


        services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommandHandler).Assembly));

        services.AddAutoMapper(typeof(OrderProfile).Assembly);
        return services;
    }
}

