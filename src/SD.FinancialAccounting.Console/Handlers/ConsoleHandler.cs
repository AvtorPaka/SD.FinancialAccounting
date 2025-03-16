using Microsoft.Extensions.Configuration;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers;

public class ConsoleHandler : IConsoleHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public ConsoleHandler(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task HandleRequests(CancellationTokenSource cts)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cts.Token);
        throw new NotImplementedException();
    }
}