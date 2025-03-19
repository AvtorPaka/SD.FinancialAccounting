using System.Text;
using SD.FinancialAccounting.Domain.Export.Visitors.Interfaces;
using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Domain.Export.Visitors.Concrete;

internal sealed class CsvVisitor : IDataVisitor
{
    public string FileExtensions => ".csv";

    private readonly StringBuilder _builder = new StringBuilder();

    public void Visit(BankAccountModel accountModel)
    {
        if (_builder.Length == 0)
        {
            _builder.AppendLine("Account ID;Name;Balance");
        }
        _builder.AppendLine($"{accountModel.Id};{accountModel.Name};{accountModel.Balance}");
    }

    public void Visit(OperationModel operationModel)
    {
        if (_builder.Length == 0)
        {
            _builder.AppendLine("Operation ID;Account ID;Category ID;Operation amount;Date;Description");
        }
        _builder.AppendLine(
            $"{operationModel.Id};{operationModel.BankAccountId};{operationModel.Category.Id};{operationModel.Amount};{operationModel.Date:o};{(operationModel.Description.Length == 0 ? "None" : operationModel.Description)}"
        );
    }

    public void Visit(OperationCategoryModel categoryModel)
    {
        if (_builder.Length == 0)
        {
            _builder.AppendLine("Category ID;Type;Name");
        }
        _builder.AppendLine($"{categoryModel.Id};{categoryModel.Type};{categoryModel.Name}");
    }

    public string GetResult()
    {
        return _builder.ToString().TrimEnd();
    }
}