using SD.FinancialAccounting.Console.Contracts.Requests.Operation;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Domain.Containers;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Console.Controllers;

internal sealed class OperationController
{
    private readonly IOperationService _operationService;

    public OperationController(IOperationService operationService)
    {
        _operationService = operationService;
    }

    internal async Task<ControllerResponse> CreateNewOperation(CreateOperationRequest request,
        CancellationToken cancellationToken)
    {
        long createdId = await _operationService.CreateOperation(
            container: new CreateOperationContainer(
                BankAccountId: request.BankAccountId,
                Amount: request.Amount,
                OperationCategoryId: request.OperationCategoryId,
                Description: request.Description ?? "",
                Date: DateTimeOffset.UtcNow
            ),
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: $"\nCreated operation id: {createdId}"
        );
    }

    // Edit methods are separated due to significant differences in the logic of change processing.

    internal async Task<ControllerResponse> EditOperationDescription(EditOperationDescriptionRequest request,
        CancellationToken cancellationToken)
    {
        await _operationService.EditOperationDescription(
            newDescription: request.Description,
            id: request.Id,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: $"\nOperation with Id : {request.Id} edited\n"
        );
    }

    internal async Task<ControllerResponse> EditOperationAmount(EditOperationAmountRequest request,
        CancellationToken cancellationToken)
    {
        var container = await _operationService.EditOperationAmount(
            newAmount: request.NewAmount,
            id: request.Id,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: $"Operation with Id : {request.Id} edited.\nAffected accounts (ids) : {container.AffectedAccountId[0]}"
        );
    }

    internal async Task<ControllerResponse> EditOperationCategory(EditOperationCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var container = await _operationService.EditOperationCategoryId(
            newCategoryId: request.NewOperationCategoryId,
            id: request.Id,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: $"Operation with Id : {request.Id} edited.\nAffected accounts (ids) : {container.AffectedAccountId[0]}"
        );
    }

    internal async Task<ControllerResponse> EditOperationBankAccount(EditOperationBankAccountIdRequest request,
        CancellationToken cancellationToken)
    {
        var container = await _operationService.EditOperationBankAccountId(
            newAccountId: request.NewBankAccountId,
            id: request.Id,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body:
            $"Operation with Id : {request.Id} edited.\nAffected accounts (ids) : [{container.AffectedAccountId[0]}, {container.AffectedAccountId[1]}]"
        );
    }

    internal async Task<ControllerResponse> DeleteOperation(DeleteOperationRequest request,
        CancellationToken cancellationToken)
    {
        long affectedAccId = await _operationService.DeleteOperation(
            id: request.Id,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: $"\nOperation with id : {request.Id} deleted.\nAffected accounts (ids) : {affectedAccId}"
        );
    }

    internal async Task<ControllerResponse> GetAllOperations(CancellationToken cancellationToken)
    {
        var operations = await _operationService.GetOperations(cancellationToken);

        return new ControllerResponse(
            body: operations.ConvertToString()
        );
    }

    internal async Task<ControllerResponse> GetAccountOperations(GetOperationsByAccountRequest request,
        CancellationToken cancellationToken)
    {
        var operations = await _operationService.GetOperationByBankAccountId(
            bankAccountId: request.BankAccountId,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: operations.ConvertToString()
        );
    }
}