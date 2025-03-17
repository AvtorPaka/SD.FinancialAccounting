using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Contracts.Requests.Account;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Controllers;
using SD.FinancialAccounting.Console.Enums;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers.ActionHandlers;

internal class AccountActionHandler: ActionHandlerBase
{
    public AccountActionHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    internal override async Task<ResponseBase> HandleAction(CancellationToken cancellationToken)
    {
        ConsoleUiHelpers.PrintAccountSectionMenu();
        AccountSectionAction accountAction = (AccountSectionAction)ConsoleHelper.ReadKeyInRange(1, 6);

        await using var scope = Services.CreateAsyncScope();
        var controller = scope.ServiceProvider.GetRequiredService<BankAccountController>();

        return accountAction switch
        {
            AccountSectionAction.GetAll => await controller.GetAccounts(cancellationToken),
            AccountSectionAction.Create => await HandleCreate(controller, cancellationToken),
            AccountSectionAction.Edit => await HandleEdit(controller, cancellationToken),
            AccountSectionAction.Delete => await HandleDelete(controller, cancellationToken),
            AccountSectionAction.Export => await HandleExport(controller, cancellationToken),
            AccountSectionAction.Cancel => new ControllerResponse(""),
            _ => throw new ArgumentException("Unsupported account action type")
        };
    }

    // TODO: Rework
    private static async Task<ResponseBase> HandleExport(BankAccountController controller, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        ConsoleUiHelpers.PrintExportSubSectionMenu();
        ExportType exportType = (ExportType)ConsoleHelper.ReadKeyInRange(1, 4);

        if (exportType == ExportType.Cancel)
        {
            return new ControllerResponse("");
        }
                
        return new ControllerResponse("Export bla bla");
    }

    private static async Task<ResponseBase> HandleCreate(BankAccountController controller, CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account name:");
        string accountName = System.Console.ReadLine() ?? throw new ArgumentException("Incorrect account name");

        return await controller.CreateNewAccount(
            new CreateAccountRequest(
                Name: accountName
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEdit(BankAccountController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account id:");
        long editAccountId = ConsoleHelper.ReadLong();
                
        System.Console.WriteLine(">>Input new bank account name:");
        string editAccountName =
            System.Console.ReadLine() ?? throw new ArgumentException("Incorrect account name");

        return await controller.EditAccount(
            new EditAccountRequest(
                Id: editAccountId,
                NewName: editAccountName
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleDelete(BankAccountController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account id:");
        long deleteAccountId = ConsoleHelper.ReadLong();

        return await controller.DeleteAccount(
            new DeleteAccountRequest(
                Id: deleteAccountId
            ),
            cancellationToken: cancellationToken
        );
    }
}