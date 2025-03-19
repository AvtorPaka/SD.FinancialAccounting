using SD.FinancialAccounting.Domain.Export.Enums;

namespace SD.FinancialAccounting.Console.Contracts.Requests;

public record ExportDataRequest(
    ExportType Type,
    string PathToExport
);