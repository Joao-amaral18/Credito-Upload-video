using System.Reflection;
using CodenApp.Sdk.Domain.Abstraction.Events;
using CodenApp.Sdk.Infrastructure.Abstraction.Bus;
using Crefaz.Credito.Infra.CrossCutting.Bus;
using Crefaz.Credito.Infra.CrossCutting.IocExtensions;
using Crefaz.Credito.Infra.CrossCutting.Logger;
using Crefaz.Documento.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Crefaz.Documento.Infra.Data.Context;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Crefaz.Documento.Domain.Entities;
using Crefaz.Documento.Application.Services;
using Crefaz.Documento.Infra.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddSwaggerConfig("VideoUpload API", "V1");

builder.Services.AddScoped<IEventStore, InConsoleEventStore>();
builder.Services.AddScoped<IMediatorHandler, InMemoryBus>();
builder.Services.AddScoped<IVideoService, VideoUploadService>();
builder.Services.AddScoped<IVideoRepository, VideoRepository>();

string azureVaultUrl = builder.Configuration["KeyVault:Url"];

AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
KeyVaultClient keyVaultClient = new(
    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback)
);
builder.Configuration.AddAzureKeyVault(
    azureVaultUrl,
    keyVaultClient,
    new DefaultKeyVaultSecretManager()
);
var secretBundle = await keyVaultClient.GetSecretAsync(
    $"{azureVaultUrl}secrets/CrefazConnectionString"
);
var connectionString = secretBundle.Value;

builder.Services.AddDbContext<CreditoContext>(options =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    options.UseSqlServer(connectionString);
    options.LogTo(Console.WriteLine, LogLevel.Information);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "VideoUpload API V1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();
