using SD.FinancialAccounting.Domain.Models.Enums;

namespace SD.FinancialAccounting.Domain.Models;

public record OperationCategoryModel(
    long Id,
    OperationCategoryType Type,
    string Name
)
{
    public override string ToString()
    {
        return $"Category Id : {Id}\nType : {Type.ToString()}\nName : {Name}";
    }
};