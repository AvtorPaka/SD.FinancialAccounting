using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;
using SD.FinancialAccounting.Infrastructure.Configuration.Models;
using SD.FinancialAccounting.Infrastructure.Dal.Infrastructure;
using SD.FinancialAccounting.Infrastructure.Dal.Repositories;

namespace SD.FinancialAccounting.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IOperationsRepository, OperationsRepository>();
        return services;
    }

    public static IServiceCollection AddDalInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var postgresConnectionSection = configuration.GetSection("DalOptions:PostgreOptions");

        PostgreConnectionOptions pgOptions = postgresConnectionSection.Get<PostgreConnectionOptions>() ??
                                             throw new ArgumentException("Postgre connection options is missing.");
        
        Postgres.ConfigureTypeMapOptions();
        Postgres.AddDataSource(services, pgOptions);
        Postgres.AddMigrations(services, pgOptions);

        return services;
    }
}