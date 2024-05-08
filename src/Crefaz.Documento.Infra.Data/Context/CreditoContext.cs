using Crefaz.Documento.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Crefaz.Documento.Infra.Data.Context;

public class CreditoContext : DbContext
{
    private readonly IConfiguration _configuration;
    public CreditoContext(DbContextOptions<CreditoContext> options, IConfiguration configuration)
       : base(options)
    {
        _configuration = configuration;
    }

    public CreditoContext(DbContextOptions<CreditoContext> options)
        : base(options)
    {
    }

    public DbSet<Video> video { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Log

        //Seguranca
        modelBuilder.Entity<Video>().ToTable("Video", "dbo");
        base.OnModelCreating(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(string))
                {
                    property.SetIsUnicode(false);
                }
            }
        }
    }
}