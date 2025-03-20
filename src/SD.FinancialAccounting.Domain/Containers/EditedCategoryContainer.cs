using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Domain.Containers;

public record EditedCategoryContainer(
    OperationCategoryModel EditedModel,
    IReadOnlyList<long> AffectedAccountId
);