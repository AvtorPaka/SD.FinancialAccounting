using SD.FinancialAccounting.Console.Contracts.Requests;
using SD.FinancialAccounting.Console.Contracts.Requests.Account;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Mappers;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Domain.Services.Interfaces;

namespace SD.FinancialAccounting.Console.Controllers;

internal sealed class BankAccountController
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }

    internal async Task<ControllerResponse> CreateNewAccount(CreateAccountRequest request,
        CancellationToken cancellationToken)
    {
        var model = await _bankAccountService.CreateBankAccount(request.Name, cancellationToken);

        return new ControllerResponse(
            body: $"\nCreated bank account:\n{model}"
        );
    }

    internal async Task<ControllerResponse> EditAccount(EditAccountRequest request, CancellationToken cancellationToken)
    {
        var newModel = await _bankAccountService.EditBankAccount(
            newModel: request.MapRequestToModel(),
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: $"\nEdited account:\n{newModel}"
        );
    }

    internal async Task<ControllerResponse> DeleteAccount(DeleteAccountRequest request,
        CancellationToken cancellationToken)
    {
        await _bankAccountService.DeleteBankAccount(request.Id, cancellationToken);

        return new ControllerResponse(
            body: $"\nBank account with Id: {request.Id} deleted."
        );
    }

    internal async Task<ControllerResponse> GetAccounts(CancellationToken cancellationToken)
    {
        var accounts = await _bankAccountService.GetAllAccounts(cancellationToken);

        return new ControllerResponse(
            body: accounts.ConvertToString()
        );
    }

    internal async Task<ControllerResponse> ExportAccounts(ExportDataRequest request,
        CancellationToken cancellationToken)
    {
        await _bankAccountService.ExportAccounts(
            exportType: request.Type,
            exportPath: request.PathToExport,
            cancellationToken: cancellationToken
        );

        return new ControllerResponse(
            body: "\nData exported."
        );
    }
}