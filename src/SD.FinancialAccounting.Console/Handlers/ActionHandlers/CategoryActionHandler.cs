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

    private static async Task<ResponseBase> HandleExport(OperationCategoryController controller,
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

        return await controller.ExportCategories(
            new ExportDataRequest(
                Type: (ExportType)(exportSectionType),
                PathToExport: exportPath
            ),
            cancellationToken: cancellationToken
        );
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