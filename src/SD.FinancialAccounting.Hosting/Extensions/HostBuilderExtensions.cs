using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Hosting.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureServices(this IHostBuilder hostBuilder,
        Action<IServiceCollection> configureAction)
    {
        return hostBuilder.ConfigureServices((configuration, services) => configureAction(services));
    }
    
    internal static IHostBuilder ConfigureDefaults(this IHostBuilder hostBuilder)
    {
        return hostBuilder
            .ConfigureAppConfiguration(ConfigureDefaultAppConfiguration)
            .ConfigureServices(ConfigureDefaultServices);
    }

    private static void ConfigureDefaultServices(IConfiguration appConfiguration, IServiceCollection serviceCollection)
    {
        serviceCollection.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();

            builder.AddConfiguration(appConfiguration.GetSection("Logging"));
            
            builder.Configure(options =>
            {
                options.ActivityTrackingOptions =
                    ActivityTrackingOptions.SpanId |
                    ActivityTrackingOptions.TraceId |
                    ActivityTrackingOptions.ParentId;
            });
        });
    }

    private static void ConfigureDefaultAppConfiguration(IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
    }
}