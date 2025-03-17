using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Enums;
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
        ConsoleHelper.PrintMainMenu();
        MainMenuAction sectionAction = (MainMenuAction)ConsoleHelper.ReadKeyInRange(1, 5);

        ResponseBase response;

        switch (sectionAction)
        {
            case MainMenuAction.AccountsSection:
                response = await AccountActionHandler.HandleAccountAction(_serviceProvider, cts.Token);
                break;
            case MainMenuAction.OperationsSection:
                response = await OperationActionHandler.HandleAccountAction(_serviceProvider, cts.Token);
                break;
            case MainMenuAction.CategoriesSection:
                response = new ControllerResponse("Categ section");
                break;
            case MainMenuAction.AnalyticsSection:
                response = new ControllerResponse("Analytics section");
                break;
            case MainMenuAction.Exit:
                response = new ControllerResponse("");
                await cts.CancelAsync();
                break;
            default:
                throw new ArgumentException("Unsupported menu action type");
        }

        return response;
    }
}