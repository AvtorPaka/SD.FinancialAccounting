using SD.FinancialAccounting.Console.Enums;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Abstraction;

namespace SD.FinancialAccounting.Console.Handlers;

internal class ActionHandlerFactory
{
    private readonly IServiceProvider _services;

    public ActionHandlerFactory(IServiceProvider services)
    {
        _services = services;
    }

    public ActionHandlerBase GetHandler(MainMenuAction actionType)
    {
        ActionHandlerBase actionHandler = actionType switch
        {
            MainMenuAction.AccountsSection => new AccountActionHandler(_services),
            MainMenuAction.OperationsSection => new OperationActionHandler(_services),
            MainMenuAction.CategoriesSection => new CategoryActionHandler(_services),
            MainMenuAction.AnalyticsSection => new ExitActionHandler(_services), //TODO: Rework
            MainMenuAction.Exit => new ExitActionHandler(_services),
            _ => throw new ArgumentException("Unsupported menu action type")
        };

        return actionHandler;
    }
}