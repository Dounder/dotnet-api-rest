using System.Reflection;
using Core.Interfaces.Common;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Context;

public class AppDbContext : IdentityDbContext
{
    private readonly IConfiguration configuration;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
    {
        this.configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entity in builder.Model.GetEntityTypes()
                     .Where(e => typeof(IBaseEntity).IsAssignableFrom(e.ClrType)))
        {
            builder.Entity(entity.Name).Property(nameof(IBaseEntity.Iid))
                .ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
        }

        SeedManager.Seed(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
}