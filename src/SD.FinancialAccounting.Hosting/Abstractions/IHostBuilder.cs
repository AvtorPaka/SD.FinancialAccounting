using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Hosting.Exceptions;

namespace SD.FinancialAccounting.Hosting.Abstractions;

public interface IHostBuilder
{
    public IHostBuilder ConfigureServices(Action<IConfiguration, IServiceCollection>? configureAction);

    public IHostBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder>? configureAction);

    public IHostBuilder ConfigureExceptionHandler(Action<ExceptionContext> handler);

    public IHost Build();
}