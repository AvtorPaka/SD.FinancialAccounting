using SD.FinancialAccounting.Domain.Models.Enums;
using SD.FinancialAccounting.Domain.Models.Interfaces;

namespace SD.FinancialAccounting.Domain.Models;

public record OperationCategoryModel(
    long Id,
    OperationCategoryType Type,
    string Name
): IExportable
{
    public override string ToString()
    {
        return $"Category Id : {Id}\nType : {Type.ToString()}\nName : {Name}";
    }
};