using SD.FinancialAccounting.Console.Enums;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers;

public class ConsoleHandler : IConsoleHandler
{
    private readonly IServiceProvider _serviceProvider;

    public ConsoleHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ResponseBase> HandleRequests(CancellationTokenSource cts)
    {
        ConsoleUiHelpers.PrintMainMenu();
        MainMenuAction sectionAction = (MainMenuAction)ConsoleHelper.ReadKeyInRange();


        ActionHandlerBase actionHandler = sectionAction switch
        {
            MainMenuAction.AccountsSection => new AccountActionHandler(_serviceProvider),
            MainMenuAction.OperationsSection => new OperationActionHandler(_serviceProvider),
            MainMenuAction.CategoriesSection => new CategoryActionHandler(_serviceProvider),
            MainMenuAction.AnalyticsSection => new ExitActionHandler(_serviceProvider), //TODO: Rework
            MainMenuAction.Exit => new ExitActionHandler(_serviceProvider),
            _ => throw new ArgumentException("Unsupported menu action type")
        };

        if (sectionAction == MainMenuAction.Exit)
        {
            await cts.CancelAsync();
        }
        
        return await actionHandler.HandleAction(cts.Token);
    }
}