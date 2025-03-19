using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Contracts.Requests;
using SD.FinancialAccounting.Console.Contracts.Requests.Account;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Controllers;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Abstraction;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Enums;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Domain.Export.Enums;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers.ActionHandlers;

internal class AccountActionHandler : ActionHandlerBase
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
        var decorator = scope.ServiceProvider.GetRequiredService<ControllerActionTimerDecorator>();

        return accountAction switch
        {
            AccountSectionAction.GetAll => await HandleGetAll(controller, decorator, cancellationToken),
            AccountSectionAction.Create => await HandleCreate(controller, decorator, cancellationToken),
            AccountSectionAction.Edit => await HandleEdit(controller, decorator, cancellationToken),
            AccountSectionAction.Delete => await HandleDelete(controller, decorator, cancellationToken),
            AccountSectionAction.Export => await HandleExport(controller, decorator, cancellationToken),
            AccountSectionAction.Cancel => new ControllerResponse(""),
            _ => throw new ArgumentException("Unsupported account action type")
        };
    }

    private static async Task<ResponseBase> HandleGetAll(
        BankAccountController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        ControllerAction action = async (request, token) =>
            await controller.GetAccounts(
                (EmptyRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            request: new EmptyRequest(),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleExport(
        BankAccountController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        ConsoleUiHelpers.PrintExportSubSectionMenu();
        ExportSectionType exportSectionType = (ExportSectionType)ConsoleHelper.ReadKeyInRange(1, 4);

        if (exportSectionType == ExportSectionType.Cancel)
        {
            return new ControllerResponse("");
        }

        System.Console.WriteLine(">>Input path to export file:");
        string exportPath = System.Console.ReadLine() ?? throw new ArgumentException("Incorrect export path");

        ControllerAction action = async (request, token) =>
            await controller.ExportAccounts(
                (ExportDataRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            request: new ExportDataRequest(
                Type: (ExportType)(exportSectionType),
                PathToExport: exportPath
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleCreate(
        BankAccountController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account name:");
        string accountName = System.Console.ReadLine() ?? throw new ArgumentException("Incorrect account name");

        ControllerAction action = async (request, token) =>
            await controller.CreateNewAccount(
                (CreateAccountRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new CreateAccountRequest(
                Name: accountName
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEdit(
        BankAccountController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account id:");
        long editAccountId = ConsoleHelper.ReadLong();

        System.Console.WriteLine(">>Input new bank account name:");
        string editAccountName =
            System.Console.ReadLine() ?? throw new ArgumentException("Incorrect account name");

        ControllerAction action = async (request, token) =>
            await controller.EditAccount(
                (EditAccountRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new EditAccountRequest(
                Id: editAccountId,
                NewName: editAccountName
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleDelete(
        BankAccountController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account id:");
        long deleteAccountId = ConsoleHelper.ReadLong();

        ControllerAction action = async (request, token) =>
            await controller.DeleteAccount(
                (DeleteAccountRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new DeleteAccountRequest(
                Id: deleteAccountId
            ),
            cancellationToken: cancellationToken
        );
    }
}