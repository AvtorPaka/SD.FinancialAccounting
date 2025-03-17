using System.Text;
using SD.FinancialAccounting.Domain.Models;

namespace SD.FinancialAccounting.Console.Utils;

internal static class ConsoleHelper
{
    private static readonly string Delimiter = new string('-', 15);

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
        System.Console.TreatControlCAsInput = true;
        var keyInfo = System.Console.ReadKey(true);
        while (!(keyInfo.Key == ConsoleKey.C &&
                 (keyInfo.Modifiers & ConsoleModifiers.Control) != 0) &&
               (int.Parse(keyInfo.KeyChar.ToString()) < minRange || int.Parse(keyInfo.KeyChar.ToString()) > maxRange))
        {
            keyInfo = System.Console.ReadKey(true);
        }

        if (keyInfo.Key == ConsoleKey.C &&
            (keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
        {
            System.Console.TreatControlCAsInput = false;
            return maxRange;
        }

        System.Console.TreatControlCAsInput = false;
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