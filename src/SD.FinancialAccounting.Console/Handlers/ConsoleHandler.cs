using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Abstraction;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Enums;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers;

internal class ConsoleHandler : IConsoleHandler
{
    private readonly ActionHandlerFactory _handlerFactory;

    public ConsoleHandler(ActionHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public async Task<ResponseBase> HandleRequests(CancellationTokenSource cts)
    {
        ConsoleUiHelpers.PrintMainMenu();
        MainMenuAction sectionAction = (MainMenuAction)ConsoleHelper.ReadKeyInRange();
        
        ActionHandlerBase actionHandler = _handlerFactory.GetHandler(sectionAction);

        if (sectionAction == MainMenuAction.Exit)
        {
            await cts.CancelAsync();
        }
        
        return await actionHandler.HandleAction(cts.Token);
    }
}