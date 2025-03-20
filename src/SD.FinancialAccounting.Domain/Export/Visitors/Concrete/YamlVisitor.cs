using SD.FinancialAccounting.Domain.Export.Visitors.Interfaces;
using SD.FinancialAccounting.Domain.Export.Visitors.YmlConverters;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Models.Interfaces;
using YamlDotNet.Serialization;

namespace SD.FinancialAccounting.Domain.Export.Visitors.Concrete;

internal sealed class YamlVisitor: IDataVisitor
{
    public string FileExtensions => ".yml";
    
    private readonly List<IExportable> _exportedItems = [];
    private static readonly ISerializer YamlSerializer = new SerializerBuilder()
        .WithTypeConverter(new DateTimeOffsetConverter())
        .DisableAliases()
        .Build();
    
    public void Visit(BankAccountModel accountModel)
    {
        _exportedItems.Add(accountModel);
    }

    public void Visit(OperationModel operationModel)
    {
        _exportedItems.Add(operationModel);
    }

    public void Visit(OperationCategoryModel categoryModel)
    {
        _exportedItems.Add(categoryModel);
    }

    public string GetResult()
    {
        return YamlSerializer.Serialize(_exportedItems);
    }
}