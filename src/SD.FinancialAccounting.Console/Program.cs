﻿using SD.FinancialAccounting.Console.Extensions;
using SD.FinancialAccounting.Hosting;

namespace SD.FinancialAccounting.Console;

internal sealed class Program
{
    private static async Task Main()
    {
        var hostBuilder = Host
            .CreateDefaultBuilder()
            .ConfigureConsoleHandler();
        

        using var app = hostBuilder.Build();
        await app.RunAsync();
    }
}