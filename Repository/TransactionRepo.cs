using BytesTracker.Model;


namespace BytesTracker.Repository
{
    public abstract class TransactionRepo
    {
        public abstract Task AddTransaction(Transaction transaction);
        public abstract Task<List<Transaction>> GetTransactions(int userId);



    }
}
