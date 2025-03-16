namespace SD.FinancialAccounting.Hosting.Abstractions;

public interface IHost: IDisposable
{
    public Task RunAsync(CancellationToken cancellationToken = default);
}