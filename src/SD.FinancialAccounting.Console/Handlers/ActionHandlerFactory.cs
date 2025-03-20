using SD.FinancialAccounting.Console.Handlers.ActionHandlers;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Abstraction;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Enums;

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
            MainMenuAction.AnalyticsSection => new AnalyticsActionHandler(_services),
            MainMenuAction.Exit => new ExitActionHandler(_services),
            _ => throw new ArgumentException("Unsupported menu action type")
        };

        return actionHandler;
    }
}