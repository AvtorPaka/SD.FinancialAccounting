using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Handlers;
using SD.FinancialAccounting.Hosting.Abstractions;
using SD.FinancialAccounting.Hosting.Extensions;

namespace SD.FinancialAccounting.Console.Extensions;

internal static class HostBuilderExtensions
{
    internal static IHostBuilder ConfigureConsoleHandler(this IHostBuilder builder)
    {
        return builder.ConfigureServices(configureAction: services =>
        {
            services.AddSingleton<IConsoleHandler, ConsoleHandler>();
        });
    }
}