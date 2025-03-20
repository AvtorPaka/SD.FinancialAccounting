using Microsoft.Extensions.DependencyInjection;
using SD.FinancialAccounting.Console.Contracts.Requests.Analytics;
using SD.FinancialAccounting.Console.Contracts.Responses;
using SD.FinancialAccounting.Console.Controllers;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Abstraction;
using SD.FinancialAccounting.Console.Handlers.ActionHandlers.Enums;
using SD.FinancialAccounting.Console.Utils;
using SD.FinancialAccounting.Hosting.Abstractions;

namespace SD.FinancialAccounting.Console.Handlers.ActionHandlers;

internal sealed class AnalyticsActionHandler : ActionHandlerBase
{
    public AnalyticsActionHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    internal override async Task<ResponseBase> HandleAction(CancellationToken cancellationToken)
    {
        ConsoleUiHelpers.PrintAnalyticsSectionMenu();
        AnalyticsSectionAction analyticsSectionAction = (AnalyticsSectionAction)ConsoleHelper.ReadKeyInRange(1, 3);

        await using var scope = Services.CreateAsyncScope();
        var controller = scope.ServiceProvider.GetRequiredService<AnalyticsController>();
        var decorator = scope.ServiceProvider.GetRequiredService<ControllerActionTimerDecorator>();

        return analyticsSectionAction switch
        {
            AnalyticsSectionAction.GetAccountOperationsGrouped => await HandleAccountOperationsGrouped(controller, decorator, cancellationToken),
            AnalyticsSectionAction.GetAccountOperationsDiff => await HandleAccountOperationsDiff(controller, decorator, cancellationToken),
            AnalyticsSectionAction.Cancel => new ControllerResponse(""),

            _ => throw new ArgumentException("Unsupported account action type")
        };
    }

    private static async Task<ResponseBase> HandleAccountOperationsGrouped(
        AnalyticsController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account id:");
        long accountId = ConsoleHelper.ReadLong();

        ControllerAction action = async (request, token) =>
            await controller.GetAccountOperationsGrouped(
                (GetAccountOperationsGroupedRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            request: new GetAccountOperationsGroupedRequest(
                AccountId: accountId
            ),
            cancellationToken: cancellationToken
        );
    }

    private static async Task<ResponseBase> HandleAccountOperationsDiff(
        AnalyticsController controller,
        ControllerActionTimerDecorator timerDecorator,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine(">>Input bank account id:");
        long accountId = ConsoleHelper.ReadLong();
        System.Console.WriteLine(">>Input start date:");
        DateTimeOffset startDate = ConsoleHelper.ReadDateTimeOffset();
        System.Console.WriteLine(">>Input end date:");
        DateTimeOffset endDate = ConsoleHelper.ReadDateTimeOffset(isEndDate: true);

        ControllerAction action = async (request, token) =>
            await controller.GetAccountOperationsPeriodDifference(
                (AccountOperationsPeriodDifferenceRequest)request,
                token
            );

        timerDecorator.SetControllerAction(action);

        return await timerDecorator.ExecuteActionWithMeasuring(
            request: new AccountOperationsPeriodDifferenceRequest(
                AccountId: accountId,
                StartDate: startDate,
                EndDate: endDate
            ),
            cancellationToken: cancellationToken
        );
    }
}