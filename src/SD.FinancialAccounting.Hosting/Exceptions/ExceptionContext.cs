using Microsoft.Extensions.Logging;

namespace SD.FinancialAccounting.Hosting.Exceptions;

public class ExceptionContext
{
    private readonly Exception? _exception;
    private readonly ILogger<ExceptionContext> _logger;

    public ExceptionContext(Exception? exception, ILogger<ExceptionContext> logger)
    {
        _exception = exception;
        _logger = logger;
    }
    
    public ILogger<ExceptionContext> Logger => _logger;
    public Exception? Exception => _exception;
}