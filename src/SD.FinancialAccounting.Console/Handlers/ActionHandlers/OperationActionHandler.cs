using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Contracts.Requests.Operation;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Controllers;
using SD.FinancialAccounting.Console.Enums;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers.ActionHandlers;

internal class OperationActionHandler : ActionHandlerBase
{
    public OperationActionHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    internal override async Task<ResponseBase> HandleAction(CancellationToken cancellationToken)
    {
        ConsoleUiHelpers.PrintOperationSectionMenu();
        OperationSectionAction operationAction = (OperationSectionAction)ConsoleHelper.ReadKeyInRange(1, 7);

        await using var scope = Services.CreateAsyncScope();
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
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        ConsoleUiHelpers.PrintExportSubSectionMenu();
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
        ConsoleUiHelpers.PrintEditOperationSubsectionMenu();
        EditOperationAction editAction = (EditOperationAction)ConsoleHelper.ReadKeyInRange(1, 5);

        return editAction switch
        {
            EditOperationAction.Amount => await HandleEditAmount(controller, cancellationToken),
            EditOperationAction.Description => await HandleEditDescription(controller, cancellationToken),
            EditOperationAction.CategoryId => await HandleEditCategoryId(controller, cancellationToken),
            EditOperationAction.BankAccountId => await HandleEditAccountId(controller, cancellationToken),
            EditOperationAction.Cancel => new ControllerResponse(""),
            _ => throw new ArgumentException("Unsupported edit operation action type")
        };
    }

    private static async Task<ResponseBase> HandleEditDescription(OperationController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input operation id:");
        long editOperationId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input new operation description:");
        string newDesc = System.Console.ReadLine() ?? "";

        return await controller.EditOperationDescription(
            new EditOperationDescriptionRequest(
                Id: editOperationId,
                Description: newDesc
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEditAmount(OperationController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input operation id:");
        long editOperationId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input new operation amount:");
        decimal newAmount = ConsoleHelper.ReadDecimal();

        return await controller.EditOperationAmount(
            new EditOperationAmountRequest(
                Id: editOperationId,
                NewAmount: newAmount
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEditAccountId(OperationController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input operation id:");
        long editOperationId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input new bank account id:");
        long newAccountId = ConsoleHelper.ReadLong();

        return await controller.EditOperationBankAccount(
            new EditOperationBankAccountIdRequest(
                Id: editOperationId,
                NewBankAccountId: newAccountId
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEditCategoryId(OperationController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input operation id:");
        long editOperationId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input new category id:");
        long newCategoryId = ConsoleHelper.ReadLong();

        return await controller.EditOperationCategory(
            new EditOperationCategoryRequest(
                Id: editOperationId,
                NewOperationCategoryId: newCategoryId
            ),
            cancellationToken: cancellationToken
        );
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