using Microsoft.Extensions.Logging;

namespace SD.FinancialAccounting.Hosting.Extensions;

public static partial class LoggerExtensions
{
    [LoggerMessage(
        LogLevel.Information,
        Message = "[{CurTime}] Application started. Press Ctrl+C to shut down."
        )]
    internal static partial void StartHostExecution(this ILogger logger, DateTime curTime);
    
    [LoggerMessage(
        LogLevel.Information,
        Message = "[{CurTime}] Application is shutting down..."
        
    )]
    internal static partial void ShutDownHostExecution(this ILogger logger, DateTime curTime);

    [LoggerMessage(
        LogLevel.Warning,
        Message = "[{CurTime}] Exception occured during request handling : {ExceptionMessage}"
        )]
    internal static partial void LogDefaultException(this ILogger logger, DateTime curTime, string? exceptionMessage);
}