using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SD.FinancialAccounting.Hosting.Abstractions;
using SD.FinancialAccounting.Hosting.Exceptions;
using SD.FinancialAccounting.Hosting.Extensions;

namespace SD.FinancialAccounting.Hosting;

public sealed class AppHost : IHost, IAsyncDisposable
{
    private readonly IServiceProvider _appServiceProvider;
    private readonly Action<ExceptionContext> _exceptionHandler;
    private readonly ILogger<AppHost> _logger;
    private CancellationTokenSource? _cts;

    internal AppHost(IServiceProvider serviceProvider, Action<ExceptionContext> exHandler)
    {
        _appServiceProvider = serviceProvider;
        _logger = serviceProvider.GetRequiredService<ILogger<AppHost>>();
        _exceptionHandler = exHandler;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        if (_cts != null)
        {
            throw new InvalidOperationException("Unable to launch host. Host is not supposed to run several times.");
        }

        _cts = new CancellationTokenSource();

        CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            _cts.Token
        );

        Console.CancelKeyPress += OnCancelKeyPress;

        IConsoleHandler handler = _appServiceProvider.GetRequiredService<IConsoleHandler>();

        _logger.StartHostExecution(DateTime.Now);

        while (!linkedCts.Token.IsCancellationRequested)
        {
            try
            {
                await handler.HandleRequests(linkedCts);
            }
            catch (OperationCanceledException)
            {
                // Supposed to happen due to cancellation
            }
            catch (Exception ex)
            {
                _exceptionHandler(new ExceptionContext(
                    exception: ex,
                    logger: _appServiceProvider.GetRequiredService<ILogger<ExceptionContext>>())
                );
            }
        }

        _logger.ShutDownHostExecution(DateTime.Now);
        Console.CancelKeyPress -= OnCancelKeyPress;
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;
        _cts?.Cancel();
    }

    #region IDisposable

    public void Dispose()
    {
        _cts?.Cancel();
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        if (_cts != null)
        {
            await DisposeAsync(_cts).ConfigureAwait(false);
            _cts = null;
        }

        await DisposeAsync(_appServiceProvider).ConfigureAwait(false);
    }

    private static async ValueTask DisposeAsync(object obj)
    {
        switch (obj)
        {
            case IAsyncDisposable asyncDisposable:
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                break;
            case IDisposable disposable:
                disposable.Dispose();
                break;
        }
    }

    #endregion
}