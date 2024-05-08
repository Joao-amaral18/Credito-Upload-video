using CodenApp.Sdk.Infrastructure.Abstraction.Bus;
using Crefaz.Credito.Infra.CrossCutting.Bus;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class BusExtension
{
    public static IServiceCollection AddBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMediatorHandler, InMemoryBus>();
        return services;
    }
}