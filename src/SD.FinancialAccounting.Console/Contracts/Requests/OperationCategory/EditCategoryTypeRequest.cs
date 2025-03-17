using SD.FinancialAccounting.Domain.Models.Enums;

namespace SD.FinancialAccounting.Console.Contracts.Requests.OperationCategory;

public record EditCategoryTypeRequest(
    long Id,
    OperationCategoryType NewType
);