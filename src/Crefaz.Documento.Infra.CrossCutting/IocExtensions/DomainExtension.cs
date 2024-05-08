using CodenApp.Sdk.Domain.Abstraction.Events;
using CodenApp.Sdk.Domain.Notifications;
using Crefaz.Credito.Infra.CrossCutting.Logger;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DomainExtension
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBus(configuration);

        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        services.AddScoped<IEventStore, InConsoleEventStore>();

        return services;
    }
}