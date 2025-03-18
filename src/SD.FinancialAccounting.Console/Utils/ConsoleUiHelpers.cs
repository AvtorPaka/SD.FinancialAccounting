using SD.FinancialAccounting.Domain.Models.Enums;

namespace SD.FinancialAccounting.Console.Utils;

internal static class ConsoleUiHelpers
{
    private static readonly string Delimiter = new string('-', 30);

    internal static void PrintMainMenu()
    {
        System.Console.WriteLine($"\nMenu\n{Delimiter}\n[1]. Accounts\n[2]. Operations\n[3]. Categories\n[4]. Analytics\n[5]. Exit\n{Delimiter}");
    }
    
    internal static void PrintAccountSectionMenu()
    {
        System.Console.WriteLine($"\nBank Accounts Section\n{Delimiter}\n[1]. Get all accounts\n[2]. Create new account\n[3]. Edit account\n[4]. Delete account\n[5]. Export\n[6]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintOperationSectionMenu()
    {
        System.Console.WriteLine($"\nOperations Section\n{Delimiter}\n[1]. Get all operations\n[2]. Get operations by account id\n[3]. Create new operation\n[4]. Edit operation\n[5]. Delete operation\n[6]. Export\n[7]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintEditOperationSubsectionMenu()
    {
        System.Console.WriteLine($"\nEdit Operation Section\n{Delimiter}\n[1]. Edit description\n[2]. Edit amount\n[3]. Edit bank account Id\n[4]. Edit category id\n[5]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintCategorySectionMenu()
    {
        System.Console.WriteLine($"\nOperation Category Section\n{Delimiter}\n[1]. Get all categories\n[2]. Create new category\n[3]. Edit category\n[4]. Delete category\n[5]. Export\n[6]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintEditCategorySubsectionMenu()
    {
        System.Console.WriteLine($"\nEdit Category Section\n{Delimiter}\n[1]. Edit name\n[2]. Edit type\n[3]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintAnalyticsSectionMenu()
    {
        System.Console.WriteLine($"\nAnalytics Section\n{Delimiter}\n[1]. \n[2]. \n[3]. \n[4]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintExportSubSectionMenu()
    {
        System.Console.WriteLine($"\nExport Section\n{Delimiter}\n[1]. CSV\n[2]. JSON\n[3]. YAML\n[4]. Cancel\n{Delimiter}");
    }
    
    internal static void PrintCategoryTypes()
    {
        foreach (OperationCategoryType catType in Enum.GetValues<OperationCategoryType>())
        {
            System.Console.WriteLine($"[{(int)catType}]. {catType}");
        }
    }

}