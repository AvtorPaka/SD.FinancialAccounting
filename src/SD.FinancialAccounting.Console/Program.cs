using SD.FinancialAccounting.Console.Extensions;
using SD.FinancialAccounting.Domain.DependencyInjection.Extensions;
using SD.FinancialAccounting.Hosting;
using SD.FinancialAccounting.Infrastructure.Extensions;

namespace SD.FinancialAccounting.Console;

internal sealed class Program
{
    private static async Task Main()
    {
        var hostBuilder = Host
            .CreateDefaultBuilder()
            .ConfigureConsoleHandler();

        hostBuilder.ConfigureServices((config, services) =>
            {
                services
                    .AddConsoleInfrastructure()
                    .AddControllers()
                    .AddDomainServices()
                    .AddDalRepositories()
                    .AddDalInfrastructure(config);
            }
        );


        using var app = hostBuilder.Build();
        await app
            .MigrateUp()
            .RunAsync();
    }
}