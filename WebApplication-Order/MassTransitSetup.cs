using MassTransit;
using WebApplication_Order.Messaging.Consumers;

namespace WebApplication_Order;
public static class MassTransitSetup
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<PaymentSucceededConsumer>();
            x.AddConsumer<PaymentFailedConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}

