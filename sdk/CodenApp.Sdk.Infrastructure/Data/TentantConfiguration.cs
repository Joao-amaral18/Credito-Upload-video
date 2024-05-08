using CodenApp.Sdk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CodenApp.Sdk.Infrastructure.Data
{
    public class TentantConfiguration : EntityConfigurationBase<Tenant, int>
    {
        public override void Configure(EntityTypeBuilder<Tenant> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            base.Configure(builder);

            builder.ToTable("Tenant");

            builder
                .Property(p => p.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(p => p.HostName)
                .HasMaxLength(250)
                .IsRequired();

        }
    }
}
