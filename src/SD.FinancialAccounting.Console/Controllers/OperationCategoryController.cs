using SD.FinancialAccounting.Console.Contracts.Requests;
using SD.FinancialAccounting.Console.Contracts.Requests.OperationCategory;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Mappers;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Console.Controllers;

internal sealed class OperationCategoryController
{
    private readonly IOperationCategoryService _operationCategoryService;

    public OperationCategoryController(IOperationCategoryService operationCategoryService)
    {
        _operationCategoryService = operationCategoryService;
    }

    internal async Task<ControllerResponse> CreateNewCategory(CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var model = await _operationCategoryService.CreateCategory(
            model: request.MapRequestToModel(),
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: $"\nCreated category:\n{model}"
        );
    }

    // Edit methods are separated due to significant differences in the logic of change processing.

    internal async Task<ControllerResponse> EditCategoryName(EditCategoryNameRequest request,
        CancellationToken cancellationToken)
    {
        var model = await _operationCategoryService.EditCategoryName(
            id: request.Id,
            newName: request.NewName,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: $"\nEdited category:\n{model}"
        );
    }

    internal async Task<ControllerResponse> EditCategoryType(EditCategoryTypeRequest request,
        CancellationToken cancellationToken)
    {
        var container = await _operationCategoryService.EditCategoryType(
            id: request.Id,
            newType: request.NewType,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body:
            $"\nEdited category:\n{container.EditedModel}\nAffected accounts (ids) [first 20]: [{string.Join(" , ", container.AffectedAccountId.Take(20))}]"
        );
    }

    internal async Task<ControllerResponse> DeleteCategory(DeleteCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var affectedAccountId = await _operationCategoryService.DeleteCategory(
            id: request.Id,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body:
            $"\nCategory with id : {request.Id} deleted.\nAffected accounts (ids) [first 20]: [{string.Join(" , ", affectedAccountId.Take(20))}]"
        );
    }

    internal async Task<ControllerResponse> GetCategories(EmptyRequest emptyRequest, CancellationToken cancellationToken)
    {
        var categories = await _operationCategoryService.GetAllCategories(cancellationToken);

        return new ControllerResponse(
            body: categories.ConvertToString()
        );
    }
    
    internal async Task<ControllerResponse> ExportCategories(ExportDataRequest request,
        CancellationToken cancellationToken)
    {
        await _operationCategoryService.ExportCategories(
            exportType: request.Type,
            exportPath: request.PathToExport,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: "\nData exported."
        );
    }
}