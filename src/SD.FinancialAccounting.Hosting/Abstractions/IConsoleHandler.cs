namespace SD.FinancialAccounting.Hosting.Abstractions;

public interface IConsoleHandler
{
    public Task HandleRequests(CancellationTokenSource cts);
}