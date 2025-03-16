using SD.FinancialAccounting.Hosting.Abstractions;
using SD.FinancialAccounting.Hosting.Extensions;

namespace SD.FinancialAccounting.Hosting;

public static class Host
{
    public static IHostBuilder CreateDefaultBuilder()
    {
        IHostBuilder hostBuilder = new AppHostBuilder();
        return hostBuilder.ConfigureDefaults();
    }
}