using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Controllers;
using SD.FinancialAccounting.Console.Handlers;

namespace SD.FinancialAccounting.Console.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddControllers(this IServiceCollection services)
    {
        services.AddScoped<BankAccountController>();
        services.AddScoped<OperationCategoryController>();
        services.AddScoped<OperationController>();
        services.AddScoped<AnalyticsController>();

        return services;
    }

    internal static IServiceCollection AddConsoleInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ActionHandlerFactory>();
        services.AddScoped<ControllerActionTimerDecorator>();
        return services;
    }
}