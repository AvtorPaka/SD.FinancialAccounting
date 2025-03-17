using SD.FinancialAccounting.Console.Contracts.Requests.Account;
using SD.FinancialAccounting.Console.Contracts.Requests.OperationCategory;
using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Console.Mappers;

internal static class RequestMappers
{
    internal static BankAccountModel MapRequestToModel(this EditAccountRequest request)
    {
        return new BankAccountModel(
            Id: request.Id,
            Name: request.NewName,
            Balance: -1
        );
    }

    internal static OperationCategoryModel MapRequestToModel(this CreateCategoryRequest request)
    {
        return new OperationCategoryModel(
            Name: request.Name,
            Type: request.Type,
            Id: -1
        );
    }
}