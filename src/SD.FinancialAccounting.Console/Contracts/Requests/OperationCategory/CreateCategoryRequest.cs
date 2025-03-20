using SD.FinancialAccounting.Domain.Models.Enums;

namespace SD.FinancialAccounting.Console.Contracts.Requests.OperationCategory;

public record CreateCategoryRequest(
    OperationCategoryType Type,
    string Name
): IRequest;