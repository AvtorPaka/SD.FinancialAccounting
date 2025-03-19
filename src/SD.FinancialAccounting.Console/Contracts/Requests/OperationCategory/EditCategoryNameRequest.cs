using SD.FinancialAccounting.Domain.Models.Enums;

namespace SD.FinancialAccounting.Console.Contracts.Requests.OperationCategory;

public record EditCategoryNameRequest(
    long Id,
    string NewName
): IRequest;