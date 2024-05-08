using CodenApp.Sdk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace CodenApp.Sdk.Infrastructure.Data
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
        {

        }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new TentantConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
