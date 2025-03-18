using System.Transactions;

namespace SD.FinancialAccounting.Domain.Contracts.Dal.Interfaces;

public interface IDbRepository
{
    public TransactionScope CreateTransactionScope(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}