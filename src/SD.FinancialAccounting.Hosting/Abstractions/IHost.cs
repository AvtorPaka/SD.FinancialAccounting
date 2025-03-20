using Microsoft.Extensions.DependencyInjection;

namespace SD.FinancialAccounting.Hosting.Abstractions;

public interface IHost: IDisposable
{
    public Task RunAsync(CancellationToken cancellationToken = default);
    
    public IServiceProvider Services { get; }
}