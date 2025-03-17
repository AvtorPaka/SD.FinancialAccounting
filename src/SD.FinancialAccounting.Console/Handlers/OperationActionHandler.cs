using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Contracts.Requests.Account;
using SD.FinancialAccounting.Console.Contracts.Requests.Operation;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Controllers;
using SD.FinancialAccounting.Console.Enums;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers;

internal static class OperationActionHandler
{
    internal static async Task<ResponseBase> HandleAccountAction(IServiceProvider services,
        CancellationToken cancellationToken)
    {
        System.Console.Clear();
        ConsoleHelper.PrintAccountSectionMenu();
        OperationSectionAction operationAction = (OperationSectionAction)ConsoleHelper.ReadKeyInRange(1, 7);

        await using var scope = services.CreateAsyncScope();
        var controller = scope.ServiceProvider.GetRequiredService<OperationController>();

        return operationAction switch
        {
            OperationSectionAction.GetAll => await controller.GetAllOperations(cancellationToken),
            OperationSectionAction.GetByAccount => await HandleGetByAccount(controller, cancellationToken),
            OperationSectionAction.Create => await HandleCreate(controller, cancellationToken),
            OperationSectionAction.Delete => await HandleDelete(controller, cancellationToken),
            OperationSectionAction.Export => await HandleExport(controller, cancellationToken),
            OperationSectionAction.Edit => await HandleEdit(controller, cancellationToken),
            OperationSectionAction.Cancel => new ControllerResponse(""),
            _ => throw new ArgumentException("Unsupported operation action type")
        };
    }

    private static async Task<ResponseBase> HandleGetByAccount(OperationController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account id:");
        long accountId = ConsoleHelper.ReadLong();

        return await controller.GetAccountOperations(
            new GetOperationsByAccountRequest(
                BankAccountId: accountId
            ),
            cancellationToken: cancellationToken
        );
    }

    // TODO: Rework
    private static async Task<ResponseBase> HandleExport(OperationController controller,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1));
        ConsoleHelper.PrintExportSubSectionMenu();
        ExportType exportType = (ExportType)ConsoleHelper.ReadKeyInRange(1, 4);

        if (exportType == ExportType.Cancel)
        {
            return new ControllerResponse("");
        }

        return new ControllerResponse("Export bla bla");
    }

    private static async Task<ResponseBase> HandleCreate(OperationController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account id:");
        long accountId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input category id:");
        long categoryId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input operation amount:");
        decimal operationAmount = ConsoleHelper.ReadDecimal();
        System.Console.WriteLine(">>Input operation description if needed:");
        string? operationDescription = System.Console.ReadLine();

        return await controller.CreateNewOperation(
            new CreateOperationRequest(
                BankAccountId: accountId,
                OperationCategoryId: categoryId,
                Amount: operationAmount,
                Description: operationDescription
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEdit(OperationController controller,
        CancellationToken cancellationToken)
    {
        // System.Console.WriteLine(">>Input bank account id:");
        // long editAccountId = ConsoleHelper.ReadLong();
        //
        // System.Console.WriteLine(">>Input new bank account name:");
        // string editAccountName =
        //     System.Console.ReadLine() ?? throw new ArgumentException("Incorrect account name");
        //
        // return await controller.EditAccount(
        //     new EditAccountRequest(
        //         Id: editAccountId,
        //         NewName: editAccountName
        //     ),
        //     cancellationToken: cancellationToken
        // );
        return new ControllerResponse("BAM");
    }

    private static async Task<ResponseBase> HandleDelete(OperationController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input operation id:");
        long operationId = ConsoleHelper.ReadLong();

        return await controller.DeleteOperation(
            new DeleteOperationRequest(
                Id: operationId
            ),
            cancellationToken: cancellationToken
        );
    }
}