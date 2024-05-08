using CodenApp.Sdk.Domain.Abstraction.Entities;
using CodenApp.Sdk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CodenApp.Sdk.Infrastructure.Data
{
    public class EntityConfigurationBase<TEntity, TPrimaryKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : EntityBase<TPrimaryKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.HasKey(entity => entity.Id);

            if (typeof(TEntity).BaseType.BaseType == typeof(EntityTenant<TPrimaryKey>))
            {
                builder
                    .HasOne(typeof(Tenant))
                    .WithMany()
                    .HasForeignKey("TenantId")
                    .IsRequired();

                builder
                    .Property("TenantId").HasColumnName("TenantId").HasColumnType("uniqueidentifier").IsRequired();
            }
        }
    }
}
