using SD.FinancialAccounting.Domain.Export.Enums;
using SD.FinancialAccounting.Domain.Export.Visitors.Concrete;
using SD.FinancialAccounting.Domain.Export.Visitors.Interfaces;

namespace SD.FinancialAccounting.Domain.Export.Visitors;

internal sealed class VisitorsFactory
{
    internal static IDataVisitor GetVisitor(ExportType type)
    {
        return type switch
        {
            ExportType.Json => new JsonVisitor(),
            ExportType.Csv => new CsvVisitor(),
            ExportType.Yaml => new YamlVisitor(),
            _ => throw new ArgumentException("Undefined export type")
        };
    }
}