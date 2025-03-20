using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Abstraction;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers.ActionHandlers;

internal class ExitActionHandler: ActionHandlerBase
{
    public ExitActionHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    internal override async Task<ResponseBase> HandleAction(CancellationToken cancellationToken)
    {
        return await Task.FromResult(new ControllerResponse(""));
    }
}