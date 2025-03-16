using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Hosting.Abstractions;
using SD.FinancialAccounting.Hosting.Exceptions;
using SD.FinancialAccounting.Hosting.Extensions;

namespace SD.FinancialAccounting.Hosting;

public sealed class AppHostBuilder: IHostBuilder
{
    private readonly List<Action<IConfigurationBuilder>> _configureAppConfigActions = [];
    private readonly List<Action<IConfiguration, IServiceCollection>> _configureServicesActions = [];
    private Action<ExceptionContext>? _exceptionHandler;
    private IConfiguration? _appConfiguration;
    private IServiceProvider? _appServiceProvider;
    private bool _isBuilt;
    
    public IHost Build()
    {
        if (_isBuilt)
        {
            throw new InvalidOperationException("IHost is already built.");
        }
        _isBuilt = true;
        
        InitializeAppConfiguration();
        InitializeServiceProvider();
        InitializeDefaultExceptionHandler();

        return GetHost(_appServiceProvider, _exceptionHandler!);
    }
    
    public IHostBuilder ConfigureServices(Action<IConfiguration, IServiceCollection>? configureAction = null)
    {
        ArgumentNullException.ThrowIfNull(configureAction, nameof(configureAction));
        
        _configureServicesActions.Add(configureAction);

        return this;
    }

    public IHostBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder>? configureAction = null)
    {
        ArgumentNullException.ThrowIfNull(configureAction, nameof(configureAction));
        
        _configureAppConfigActions.Add(configureAction);

        return this;
    }
    
    public IHostBuilder ConfigureExceptionHandler(Action<ExceptionContext> handler)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(handler));

        _exceptionHandler = handler;
        
        return this;
    }

    private void InitializeDefaultExceptionHandler()
    {
        _exceptionHandler ??= context => { context.Logger.LogDefaultException(DateTime.Now, context.Exception?.Message); };
    }
    

    private void InitializeAppConfiguration()
    {
        IConfigurationBuilder configurationBuilder =
            new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!);

        foreach (Action<IConfigurationBuilder> builderAction in _configureAppConfigActions)
        {
            builderAction(configurationBuilder);
        }

        ArgumentNullException.ThrowIfNull(configurationBuilder, nameof(configurationBuilder));
        _appConfiguration = configurationBuilder.Build();
    }
    
    private void InitializeServiceProvider()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConfiguration>(_ => _appConfiguration!);
        
        foreach (Action<IConfiguration, IServiceCollection> builderActions in _configureServicesActions)
        {
            builderActions(_appConfiguration!, serviceCollection);
        }
        
        ArgumentNullException.ThrowIfNull(serviceCollection, nameof(serviceCollection));
        
        serviceCollection.AddLogging();
        serviceCollection.AddOptions();
        
        _appServiceProvider = serviceCollection.BuildServiceProvider();
    }

    
    private static IHost GetHost(IServiceProvider? serviceProvider, Action<ExceptionContext> exHandler)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));

        // Dispose insurance magic
        _ = serviceProvider.GetService<IConfiguration>();
        return new AppHost(serviceProvider, exHandler);
    }
}