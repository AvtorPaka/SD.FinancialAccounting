using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Contracts.Requests;
using SD.FinancialAccounting.Console.Contracts.Requests.Operation;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Controllers;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Abstraction;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Enums;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Domain.Export.Enums;
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
        var decorator = scope.ServiceProvider.GetRequiredService<ControllerActionTimerDecorator>();

        return operationAction switch
        {
            OperationSectionAction.GetAll => await HandleGetAll(controller, decorator, cancellationToken),
            OperationSectionAction.GetByAccount => await HandleGetByAccount(controller, decorator, cancellationToken),
            OperationSectionAction.Create => await HandleCreate(controller, decorator, cancellationToken),
            OperationSectionAction.Delete => await HandleDelete(controller, decorator, cancellationToken),
            OperationSectionAction.Export => await HandleExport(controller, decorator, cancellationToken),
            OperationSectionAction.Edit => await HandleEdit(controller, decorator, cancellationToken),
            OperationSectionAction.Cancel => new ControllerResponse(""),
            _ => throw new ArgumentException("Unsupported operation action type")
        };
    }

    private static async Task<ResponseBase> HandleGetAll(
        OperationController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        ControllerAction action = async (request, token) =>
            await controller.GetAllOperations(
                (EmptyRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            request: new EmptyRequest(),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleGetByAccount(
        OperationController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account id:");
        long accountId = ConsoleHelper.ReadLong();

        ControllerAction action = async (request, token) =>
            await controller.GetAccountOperations(
                (GetOperationsByAccountRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new GetOperationsByAccountRequest(
                BankAccountId: accountId
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleExport(
        OperationController controller,
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
            await controller.ExportOperations(
                (ExportDataRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new ExportDataRequest(
                Type: (ExportType)(exportSectionType),
                PathToExport: exportPath
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleCreate(
        OperationController controller,
        ControllerActionTimerDecorator timerDecorator,
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

        ControllerAction action = async (request, token) =>
            await controller.CreateNewOperation(
                (CreateOperationRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new CreateOperationRequest(
                BankAccountId: accountId,
                OperationCategoryId: categoryId,
                Amount: operationAmount,
                Description: operationDescription
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEdit(
        OperationController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        ConsoleUiHelpers.PrintEditOperationSubsectionMenu();
        EditOperationAction editAction = (EditOperationAction)ConsoleHelper.ReadKeyInRange(1, 5);

        return editAction switch
        {
            EditOperationAction.Amount => await HandleEditAmount(controller, timerDecorator, cancellationToken),
            EditOperationAction.Description => await HandleEditDescription(controller, timerDecorator,
                cancellationToken),
            EditOperationAction.CategoryId => await HandleEditCategoryId(controller, timerDecorator, cancellationToken),
            EditOperationAction.BankAccountId => await HandleEditAccountId(controller, timerDecorator,
                cancellationToken),
            EditOperationAction.Cancel => new ControllerResponse(""),
            _ => throw new ArgumentException("Unsupported edit operation action type")
        };
    }

    private static async Task<ResponseBase> HandleEditDescription(
        OperationController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input operation id:");
        long editOperationId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input new operation description:");
        string newDesc = System.Console.ReadLine() ?? "";

        ControllerAction action = async (request, token) =>
            await controller.EditOperationDescription(
                (EditOperationDescriptionRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new EditOperationDescriptionRequest(
                Id: editOperationId,
                Description: newDesc
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEditAmount(
        OperationController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input operation id:");
        long editOperationId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input new operation amount:");
        decimal newAmount = ConsoleHelper.ReadDecimal();

        ControllerAction action = async (request, token) =>
            await controller.EditOperationAmount(
                (EditOperationAmountRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new EditOperationAmountRequest(
                Id: editOperationId,
                NewAmount: newAmount
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEditAccountId(
        OperationController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input operation id:");
        long editOperationId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input new bank account id:");
        long newAccountId = ConsoleHelper.ReadLong();

        ControllerAction action = async (request, token) =>
            await controller.EditOperationBankAccount(
                (EditOperationBankAccountIdRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new EditOperationBankAccountIdRequest(
                Id: editOperationId,
                NewBankAccountId: newAccountId
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEditCategoryId(
        OperationController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input operation id:");
        long editOperationId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input new category id:");
        long newCategoryId = ConsoleHelper.ReadLong();

        ControllerAction action = async (request, token) =>
            await controller.EditOperationCategory(
                (EditOperationCategoryRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new EditOperationCategoryRequest(
                Id: editOperationId,
                NewOperationCategoryId: newCategoryId
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleDelete(
        OperationController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input operation id:");
        long operationId = ConsoleHelper.ReadLong();

        ControllerAction action = async (request, token) =>
            await controller.DeleteOperation(
                (DeleteOperationRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new DeleteOperationRequest(
                Id: operationId
            ),
            cancellationToken: cancellationToken
        );
    }
}