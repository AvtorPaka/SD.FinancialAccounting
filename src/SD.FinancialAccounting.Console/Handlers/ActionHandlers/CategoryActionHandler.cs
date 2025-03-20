using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Contracts.Requests;
using SD.FinancialAccounting.Console.Contracts.Requests.OperationCategory;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Controllers;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Abstraction;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Enums;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Domain.Export.Enums;
using SD.FinancialAccounting.Domain.Models.Enums;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers.ActionHandlers;

internal sealed class CategoryActionHandler : ActionHandlerBase
{
    public CategoryActionHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    internal override async Task<ResponseBase> HandleAction(CancellationToken cancellationToken)
    {
        ConsoleUiHelpers.PrintCategorySectionMenu();
        CategorySectionAction categoryAction = (CategorySectionAction)ConsoleHelper.ReadKeyInRange(1, 6);

        await using var scope = Services.CreateAsyncScope();
        var controller = scope.ServiceProvider.GetRequiredService<OperationCategoryController>();
        var decorator = scope.ServiceProvider.GetRequiredService<ControllerActionTimerDecorator>();

        return categoryAction switch
        {
            CategorySectionAction.GetAll => await HandleGetAll(controller, decorator, cancellationToken),
            CategorySectionAction.Create => await HandleCreate(controller, decorator, cancellationToken),
            CategorySectionAction.Edit => await HandleEdit(controller, decorator, cancellationToken),
            CategorySectionAction.Delete => await HandleDelete(controller, decorator, cancellationToken),
            CategorySectionAction.Export => await HandleExport(controller, decorator, cancellationToken),
            CategorySectionAction.Cancel => new ControllerResponse(""),
            _ => throw new ArgumentException("Unsupported operation category action type")
        };
    }

    private static async Task<ResponseBase> HandleGetAll(
        OperationCategoryController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        ControllerAction action = async (request, token) =>
            await controller.GetCategories(
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
        OperationCategoryController controller,
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
            await controller.ExportCategories(
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
        OperationCategoryController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input category name:");
        string categoryName = System.Console.ReadLine() ?? throw new ArgumentException("Incorrect category name");
        System.Console.WriteLine(">>Choose category type:");
        ConsoleUiHelpers.PrintCategoryTypes();
        OperationCategoryType type = (OperationCategoryType)ConsoleHelper.ReadKeyInRange(1, 2);

        ControllerAction action = async (request, token) =>
            await controller.CreateNewCategory(
                (CreateCategoryRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new CreateCategoryRequest(
                Type: type,
                Name: categoryName
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEdit
    (OperationCategoryController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        ConsoleUiHelpers.PrintEditCategorySubsectionMenu();
        EditCategoryAction editAction = (EditCategoryAction)ConsoleHelper.ReadKeyInRange(1, 3);

        return editAction switch
        {
            EditCategoryAction.Name => await HandleEditName(controller, timerDecorator, cancellationToken),
            EditCategoryAction.Type => await HandleEditType(controller, timerDecorator, cancellationToken),
            EditCategoryAction.Cancel => new ControllerResponse(""),
            _ => throw new ArgumentException("Unsupported edit operation action type")
        };
    }

    private static async Task<ResponseBase> HandleEditName(
        OperationCategoryController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input category id:");
        long categoryId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input category name:");
        string categoryName = System.Console.ReadLine() ?? throw new ArgumentException("Incorrect category name");

        ControllerAction action = async (request, token) =>
            await controller.EditCategoryName(
                (EditCategoryNameRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new EditCategoryNameRequest(
                Id: categoryId,
                NewName: categoryName
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEditType(
        OperationCategoryController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input category id:");
        long categoryId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Choose new category type:");
        ConsoleUiHelpers.PrintCategoryTypes();
        OperationCategoryType type = (OperationCategoryType)ConsoleHelper.ReadKeyInRange(1, 2);

        ControllerAction action = async (request, token) =>
            await controller.EditCategoryType(
                (EditCategoryTypeRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new EditCategoryTypeRequest(
                Id: categoryId,
                NewType: type
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleDelete(
        OperationCategoryController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input category id:");
        long categoryId = ConsoleHelper.ReadLong();

        ControllerAction action = async (request, token) =>
            await controller.DeleteCategory(
                (DeleteCategoryRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            new DeleteCategoryRequest(
                Id: categoryId
            ),
            cancellationToken: cancellationToken
        );
    }
}