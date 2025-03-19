using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SD.FinancialAccounting.Console.Contracts.Requests;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers;

public delegate Task<ResponseBase> ControllerAction(IRequest request, CancellationToken cancellationToken);

public class ControllerActionTimerDecorator
{
    private readonly ILogger<ControllerActionTimerDecorator> _logger;
    private ControllerAction? ControllerAction { get; set; }

    public ControllerActionTimerDecorator(ILogger<ControllerActionTimerDecorator> logger)
    {
        _logger = logger;
    }

    public void SetControllerAction(ControllerAction action)
    {
        ControllerAction = action;
    }

    public async Task<ResponseBase> ExecuteActionWithMeasuring(IRequest request, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        
        if (ControllerAction == null)
        {
            throw new ArgumentException("Controller action does not set.");
        }
        
        var response = await ControllerAction(request, cancellationToken);
        stopwatch.Stop();
        _logger.LogInformation("Elapsed execution time (ms): {ms}", stopwatch.ElapsedMilliseconds);
        return response;
    }
    
}