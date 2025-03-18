using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers.ActionHandlers.Abstraction;

internal abstract class ActionHandlerBase
{
    protected readonly IServiceProvider Services;

    protected ActionHandlerBase(IServiceProvider serviceProvider)
    {
        Services = serviceProvider;
    }

    internal abstract Task<ResponseBase> HandleAction(CancellationToken cancellationToken);
}