using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Crefaz.Credito.Infra.CrossCutting.IocExtensions;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, string nomeApi,
        string versaoApi, string descricaoAPI = null)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = nomeApi, Version = versaoApi, Description = descricaoAPI });
            c.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basicAuth" }
                    },
                    new string[] { }
                }
            });
        });
        return services;
    }
}