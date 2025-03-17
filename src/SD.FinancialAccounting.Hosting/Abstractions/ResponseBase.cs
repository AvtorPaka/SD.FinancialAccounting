namespace SD.FinancialAccounting.Hosting.Abstractions;

public abstract class ResponseBase
{
    protected readonly string? ResponseBody;

    protected ResponseBase(string? body)
    {
        ResponseBody = body;
    }

    public override string ToString()
    {
        return ResponseBody ?? "";
    }
}