
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

        public override async Task<List<Transaction>> TotalCredit(int userId)
        {
            return await _connection.Table<Transaction>()
                .Where(trasanction => trasanction.user_id == userId && trasanction.type.ToLower() == "credit")
                .ToListAsync();
        }

        public override async Task<List<Transaction>> TotalDebit(int userId)
        {
            return await _connection.Table<Transaction>()
                .Where(transaction => transaction.user_id == userId && transaction.type.ToLower() == "debit" )
                .ToListAsync();
        }


        public override async Task<decimal> GetTotalCredit(int userId)
        {
            var credit = await TotalCredit(userId);
            return credit.Sum(totalcredit => totalcredit.amount);
        }

        public override async Task<decimal> GetTotalDebit(int userId)
        {
            var debit = await TotalDebit(userId);
            return debit.Sum(totaldebit => totaldebit.amount);
        }





    }
}
