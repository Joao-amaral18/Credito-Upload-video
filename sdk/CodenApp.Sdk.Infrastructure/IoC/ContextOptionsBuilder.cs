using CodenApp.Sdk.Domain.Enums;
using CodenApp.Sdk.Infrastructure.Abstraction.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace CodenApp.Sdk.Infrastructure
{
    public class ContextOptionsBuilder : IContextOptionsBuilder
    {
        public string ConnectionString { get; private set; }

        public string MigrationsAssemblyName { get; private set; }

        private readonly EDatabaseType _eDatabaseType; 

        public ContextOptionsBuilder(string connectionString)
        {
            this.ConnectionString = !string.IsNullOrEmpty(connectionString)
                ? connectionString : throw new ArgumentNullException(nameof(connectionString));
        }

        public ContextOptionsBuilder(string connectionString, string migrationsAssembly)
        : this(connectionString)
        {
            this.MigrationsAssemblyName = migrationsAssembly;
        }

        public ContextOptionsBuilder(string connectionString, EDatabaseType databaseType)
        : this(connectionString)
        {
            this._eDatabaseType = databaseType;
        }

        public Action<DbContextOptionsBuilder> Builder()
        {
            Action<SqlServerDbContextOptionsBuilder> sqlOptions = null;
            if (!string.IsNullOrEmpty(this.MigrationsAssemblyName))
                sqlOptions = (options) =>
                {
                    options.MigrationsAssembly(this.MigrationsAssemblyName);
                    options.EnableRetryOnFailure();
                };

            if (this._eDatabaseType.Equals(EDatabaseType.SQLServer))
                return options => options.UseSqlServer(ConnectionString, sqlOptions);

            return options => options.UseNpgsql(ConnectionString);
        }
    }
}
