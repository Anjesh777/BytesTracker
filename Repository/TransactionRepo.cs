using BytesTracker.Model;


namespace BytesTracker.Repository
{
    public abstract class TransactionRepo
    {
        public abstract Task AddTransaction(Transaction transaction);
        public abstract Task<List<Transaction>> GetTransactions(int userId);

        public abstract Task<List<Transaction>> TotalCredit(int userId);
        public abstract Task<List<Transaction>> TotalDebit(int userId);
        public abstract Task<decimal> GetTotalCredit(int userId);
        public abstract Task<decimal> GetTotalDebit(int userId);

        public abstract Task UpdateTransaction(Transaction transaction, Guid id);
        public abstract Task<List<Transaction>> GetSortedTransaction(int userId, BytesTracker.Dto.SortFormDTO sortFormDTO);

    }
}
