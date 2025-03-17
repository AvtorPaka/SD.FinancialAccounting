using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Domain.Containers;

public record EditedOperationContainer(
    IReadOnlyList<long> AffectedAccountId
);