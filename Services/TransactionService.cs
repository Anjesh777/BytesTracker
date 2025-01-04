
using BytesTracker.Model;
using BytesTracker.Repository;
using SQLite;

namespace BytesTracker.Services
{
    public class TransactionService : TransactionRepo
    {

        private readonly SQLiteAsyncConnection _connection;

        public TransactionService(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }
        public override async Task AddTransaction(Transaction transaction)
        {
            await _connection.InsertAsync(transaction);
        }
        public override async Task<List<Transaction>> GetTransactions(int userID)
        {
            try
            {
                return await _connection.Table<Transaction>()
                    .Where(transaction => transaction.user_id == userID)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error fetching tags: " + e.Message);
            }

        }



    }
}
