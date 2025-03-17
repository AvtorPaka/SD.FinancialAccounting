using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Controllers;

namespace SD.FinancialAccounting.Console.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddControllers(this IServiceCollection services)
    {
        services.AddScoped<BankAccountController>();
        services.AddScoped<OperationCategoryController>();
        services.AddScoped<OperationController>();

        return services;
    }
}