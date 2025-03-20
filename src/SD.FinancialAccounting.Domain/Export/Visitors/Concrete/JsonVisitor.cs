using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SD.FinancialAccounting.Domain.Export.Visitors.Interfaces;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Models.Interfaces;

namespace SD.FinancialAccounting.Domain.Export.Visitors.Concrete;

internal sealed class JsonVisitor: IDataVisitor
{
    public string FileExtensions => ".json";
    
    private static readonly JsonSerializerSettings SerializerSettings= new JsonSerializerSettings()
    {
        Converters = new List<JsonConverter> { new StringEnumConverter() }
    };

    private readonly List<IExportable> _exportedItems = [];
    
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
        return JsonConvert.SerializeObject(_exportedItems, Formatting.Indented, SerializerSettings);
    }
}