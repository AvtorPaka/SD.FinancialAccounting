using SD.FinancialAccounting.Domain.Exceptions;
using SD.FinancialAccounting.Domain.Export.Enums;
using SD.FinancialAccounting.Domain.Export.Visitors;
using SD.FinancialAccounting.Domain.Export.Visitors.Interfaces;
using SD.FinancialAccounting.Domain.Models;
using SD.FinancialAccounting.Domain.Models.Interfaces;

namespace SD.FinancialAccounting.Domain.Export;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

internal sealed class DataExporter
{
    private static readonly Lazy<DataExporter> _instance =
        new Lazy<DataExporter>(() => new DataExporter());

    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    private DataExporter()
    {
    }

    public static DataExporter Instance => _instance.Value;

    internal async Task ExportAsync<T>(IReadOnlyList<T> models, ExportType exportType, string exportPath, CancellationToken cancellationToken)
        where T : IExportable
    {
        IDataVisitor visitor = VisitorsFactory.GetVisitor(exportType);
        
        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            var finalPath = PrepareFilePath(exportPath, visitor.FileExtensions);


            foreach (var model in models)
            {
                switch (model)
                {
                    case BankAccountModel account:
                        visitor.Visit(account);
                        break;
                    case OperationCategoryModel category:
                        visitor.Visit(category);
                        break;
                    case OperationModel operation:
                        visitor.Visit(operation);
                        break;
                    default:
                        throw new ArgumentException($"Unsupported type: {typeof(T).Name}");
                }
            }

            string content = visitor.GetResult();

            await WriteToFileAsync(finalPath, content);
        }
        catch (Exception ex)
        {
            throw new ExportException("Export failed", ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private string PrepareFilePath(string originalPath, string expectedExtension)
    {
        var dir = Path.GetDirectoryName(originalPath);
        var fileName = Path.GetFileNameWithoutExtension(originalPath);

        if (string.IsNullOrWhiteSpace(dir))
        {
            dir = Directory.GetCurrentDirectory();
        }

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var finalPath = Path.Combine(dir, $"{fileName}{expectedExtension}");

        return finalPath;
    }

    private async Task WriteToFileAsync(string path, string content)
    {
        try
        {
            await using var writer = new StreamWriter(path, append: false); // append: false - перезапись файла
            await writer.WriteAsync(content);
        }
        catch (Exception ex)
        {
            throw new ExportException($"Failed to export data. Failed to write to the file for path: {path}",ex);
        }
    }
}