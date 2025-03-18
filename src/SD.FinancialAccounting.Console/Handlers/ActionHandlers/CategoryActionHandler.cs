using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Contracts.Requests.OperationCategory;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Controllers;
using SD.FinancialAccounting.Console.Enums;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Abstraction;
using SD.FinancialAccounting.Console.Utils;
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

        return categoryAction switch
        {
            CategorySectionAction.GetAll => await controller.GetCategories(cancellationToken),
            CategorySectionAction.Create => await HandleCreate(controller, cancellationToken),
            CategorySectionAction.Edit => await HandleEdit(controller, cancellationToken),
            CategorySectionAction.Delete => await HandleDelete(controller, cancellationToken),
            CategorySectionAction.Export => await HandleExport(controller, cancellationToken),
            CategorySectionAction.Cancel => new ControllerResponse(""),
            _ => throw new ArgumentException("Unsupported operation category action type")
        };
    }

    // TODO: Rework
    private static async Task<ResponseBase> HandleExport(OperationCategoryController controller,
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

    private static async Task<ResponseBase> HandleCreate(OperationCategoryController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input category name:");
        string categoryName = System.Console.ReadLine() ?? throw new ArgumentException("Incorrect category name");
        System.Console.WriteLine(">>Choose category type:");
        ConsoleUiHelpers.PrintCategoryTypes();
        OperationCategoryType type = (OperationCategoryType)ConsoleHelper.ReadKeyInRange(1, 2);

        return await controller.CreateNewCategory(
            new CreateCategoryRequest(
                Type: type,
                Name: categoryName
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEdit(OperationCategoryController controller,
        CancellationToken cancellationToken)
    {
        ConsoleUiHelpers.PrintEditCategorySubsectionMenu();
        EditCategoryAction editAction = (EditCategoryAction)ConsoleHelper.ReadKeyInRange(1, 3);

        return editAction switch
        {
            EditCategoryAction.Name => await HandleEditName(controller, cancellationToken),
            EditCategoryAction.Type => await HandleEditType(controller, cancellationToken),
            EditCategoryAction.Cancel => new ControllerResponse(""),
            _ => throw new ArgumentException("Unsupported edit operation action type")
        };
    }

    private static async Task<ResponseBase> HandleEditName(OperationCategoryController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input category id:");
        long categoryId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input category name:");
        string categoryName = System.Console.ReadLine() ?? throw new ArgumentException("Incorrect category name");

        return await controller.EditCategoryName(
            new EditCategoryNameRequest(
                Id: categoryId,
                NewName: categoryName
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleEditType(OperationCategoryController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input category id:");
        long categoryId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Choose new category type:");
        ConsoleUiHelpers.PrintCategoryTypes();
        OperationCategoryType type = (OperationCategoryType)ConsoleHelper.ReadKeyInRange(1, 2);

        return await controller.EditCategoryType(
            new EditCategoryTypeRequest(
                Id: categoryId,
                NewType: type
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleDelete(OperationCategoryController controller,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input category id:");
        long categoryId = ConsoleHelper.ReadLong();

        return await controller.DeleteCategory(
            new DeleteCategoryRequest(
                Id: categoryId
            ),
            cancellationToken: cancellationToken
        );
    }
}