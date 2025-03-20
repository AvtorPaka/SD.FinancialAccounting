namespace SD.FinancialAccounting.Hosting.Abstractions;

public interface IConsoleHandler
{
    public Task<ResponseBase> HandleRequests(CancellationTokenSource cts);
}