using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Contracts.Responses;

public class ControllerResponse : ResponseBase
{
    public ControllerResponse(string? body) : base(body)
    {
    }
}