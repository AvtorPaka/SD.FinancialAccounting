using System.Text;
using Microsoft.Extensions.Primitives;
using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Console.Utils;

internal static class ConsoleHelper
{
    private static readonly string Delimiter = new string('-', 15);

    internal static string ConvertToString(this IReadOnlyList<BankAccountModel> models)
    {
        StringBuilder builder = new StringBuilder($"\nBank Accounts:\n{Delimiter}\n");
        
        if (models.Count == 0)
        {
            builder.Append("Bank accounts are empty.\n");
        }

        foreach (var account in models)
        {
            builder.Append($"{account.ToString()}\n{Delimiter}\n");
        }

        return builder.ToString();
    }

    internal static string ConvertToString(this IReadOnlyList<OperationCategoryModel> models)
    {
        StringBuilder builder = new StringBuilder($"\nCategories:\n{Delimiter}\n");

        if (models.Count == 0)
        {
            builder.Append("Categories are empty.\n");
        }
        
        foreach (var account in models)
        {
            builder.Append($"{account.ToString()}\n{Delimiter}\n");
        }

        return builder.ToString();
    }

    internal static string ConvertToString(this IReadOnlyList<OperationModel> models)
    {
        StringBuilder builder = new StringBuilder($"\nOperations:\n{Delimiter}\n");
        
        if (models.Count == 0)
        {
            builder.Append("Operations are empty.\n");
        }

        foreach (var account in models)
        {
            builder.Append($"{account.ToString()}\n{Delimiter}\n");
        }

        return builder.ToString();
    }

    internal static string ConvertToString(this Dictionary<OperationCategoryModel, decimal> operationsGrouped)
    {
        StringBuilder builder = new StringBuilder();

        if (operationsGrouped.Count == 0)
        {
            builder.AppendLine("No operations");
            return builder.ToString().TrimEnd();;
        }
        
        foreach (var kv in operationsGrouped)
        {
            builder.AppendLine($"{kv.Key.Id} - {kv.Key.Name} : {kv.Value}");
        }

        return builder.ToString().TrimEnd();
    }

    internal static int ReadKeyInRange(int minRange = 1, int maxRange = 5)
    {
        System.Console.TreatControlCAsInput = true;
        
        while (true)
        {
            var keyInfo = System.Console.ReadKey(true);
            bool isCtrlC = keyInfo.Key == ConsoleKey.C && 
                           (keyInfo.Modifiers & ConsoleModifiers.Control) != 0;

            if (isCtrlC)
            {
                System.Console.TreatControlCAsInput = false;
                return maxRange;
            }

            if (char.IsDigit(keyInfo.KeyChar))
            {
                int number = int.Parse(keyInfo.KeyChar.ToString());
                if (number >= minRange && number <= maxRange)
                {
                    System.Console.TreatControlCAsInput = false;
                    return number;
                }
            }
        }
    }

    internal static long ReadLong()
    {
        long value = 0;
        bool state = long.TryParse(System.Console.ReadLine(), out value);
        while (!state)
        {
            state = long.TryParse(System.Console.ReadLine(), out value);
        }

        return value;
    }

    internal static decimal ReadDecimal()
    {
        decimal value = 0;
        bool state = decimal.TryParse(System.Console.ReadLine(), out value);
        while (!state)
        {
            state = decimal.TryParse(System.Console.ReadLine(), out value);
        }

        return value;
    }
    
    internal static DateTimeOffset ReadDateTimeOffset(bool isEndDate = false)
    {
        bool state = DateTime.TryParse(System.Console.ReadLine(), out var date);
        while (!state)
        {
            state = DateTime.TryParse(System.Console.ReadLine(), out date);
        }

        if (isEndDate)
        {
            date = date.AddDays(1);
        }
        
        return new DateTimeOffset(date, new TimeSpan(0, 0, 0));;
    }
}