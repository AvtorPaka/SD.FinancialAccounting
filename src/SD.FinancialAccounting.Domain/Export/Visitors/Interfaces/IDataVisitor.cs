using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Domain.Export.Visitors.Interfaces;

internal interface IDataVisitor
{
    string FileExtensions { get; }
    void Visit(BankAccountModel accountModel);
    void Visit(OperationModel operationModel);
    void Visit(OperationCategoryModel categoryModel);
    string GetResult();
}