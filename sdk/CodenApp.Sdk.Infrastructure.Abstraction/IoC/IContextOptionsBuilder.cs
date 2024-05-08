using Microsoft.EntityFrameworkCore;
using System;

namespace CodenApp.Sdk.Infrastructure.Abstraction.IoC
{
    public interface IContextOptionsBuilder
    {
        string ConnectionString { get; }
        string MigrationsAssemblyName { get; }
        Action<DbContextOptionsBuilder> Builder();
    }
}
