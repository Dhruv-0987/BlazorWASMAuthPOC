using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Contracts;

public static class MassTransitExtensions
{
    public static WebApplicationBuilder ConfigureMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<RabbitMqOptions>()
            .BindConfiguration(nameof(RabbitMqOptions))
            .ValidateOnStart();

        var rabbitMqOptions = builder.Services.BuildServiceProvider()
            .GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        builder.Services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();

            configurator.UsingRabbitMq((busFactoryContext, busFactoryConfigurator) =>
            {
                busFactoryConfigurator.Host(new Uri(rabbitMqOptions!.HostName), hostingOptions =>
                {
                    hostingOptions.Username(rabbitMqOptions.UserName);
                    hostingOptions.Password(rabbitMqOptions.Password);
                });

                busFactoryConfigurator.ConfigureEndpoints(busFactoryContext);
            });
        });

        return builder;
    }
}