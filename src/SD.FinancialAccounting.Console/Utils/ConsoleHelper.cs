using System.Text;
using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Console.Utils;

internal static class ConsoleHelper
{
    private static readonly string Delimiter = new string('-', 30);

    internal static void PrintMainMenu()
    {
        System.Console.WriteLine($"\nMenu\n{Delimiter}\n[1]. Accounts\n[2]. Operations\n[3]. Categories\n[4]. Analytics\n[5]. Exit\n{Delimiter}");
    }
    
    internal static void PrintAccountSectionMenu()
    {
        System.Console.WriteLine($"\nBank Accounts Section\n{Delimiter}\n[1]. Gel all accounts\n[2]. Create new account\n[3]. Edit account\n[4]. Delete account\n[5]. Export\n[6]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintOperationSectionMenu()
    {
        System.Console.WriteLine($"\nOperations Section\n{Delimiter}\n[1]. Gel all operations\n[2]. Get operations by account id\n[3]. Create new operation\n[4]. Edit operation\n[5]. Delete operation\n[6]. Export\n[7]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintCategorySectionMenu()
    {
        System.Console.WriteLine($"\nOperation Category Section\n{Delimiter}\n[1]. Gel all categories\n[2]. Create new category\n[3]. Edit category\n[4]. Delete category\n[5]. Export\n[6]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintAnalyticsSectionMenu()
    {
        System.Console.WriteLine($"\nAnalytics Section\n{Delimiter}\n[1]. \n[2]. \n[3]. \n[4]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintExportSubSectionMenu()
    {
        System.Console.WriteLine($"\nExport Section\n{Delimiter}\n[1]. CSV\n[2]. JSON\n[3]. YAML\n[4]. Cancel\n{Delimiter}");
    }

    internal static string ConvertToString(this IReadOnlyList<BankAccountModel> models)
    {
        StringBuilder builder = new StringBuilder("\nBank Accounts:\n");

        foreach (var account in models)
        {
            builder.Append($"{account.ToString()}\n{Delimiter}\n");
        }

        return builder.ToString();
    }
    
    internal static string ConvertToString(this IReadOnlyList<OperationCategoryModel> models)
    {
        StringBuilder builder = new StringBuilder("\nCategories:\n");

        foreach (var account in models)
        {
            builder.Append($"{account.ToString()}\n{Delimiter}\n");
        }

        return builder.ToString();
    }
    
    internal static string ConvertToString(this IReadOnlyList<OperationModel> models)
    {
        StringBuilder builder = new StringBuilder("\nOperations:\n");

        foreach (var account in models)
        {
            builder.Append($"{account.ToString()}\n{Delimiter}\n");
        }

        return builder.ToString();
    }

    internal static int ReadKeyInRange(int minRange = 1, int maxRange = 5)
    {
        var keyInfo = System.Console.ReadKey(true);
        while (int.Parse(keyInfo.KeyChar.ToString()) < minRange || int.Parse(keyInfo.KeyChar.ToString()) > maxRange)
        {
            keyInfo = System.Console.ReadKey(true);
        }

        return int.Parse(keyInfo.KeyChar.ToString());
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
}