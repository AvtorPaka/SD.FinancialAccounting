using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Domain.Services;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Domain.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IBankAccountService, BankAccountService>();
        services.AddScoped<IOperationCategoryService, OperationCategoryService>();
        services.AddScoped<IOperationService, OperationService>();
        services.AddScoped<IBalanceService, BalanceService>();
        return services;
    }
}