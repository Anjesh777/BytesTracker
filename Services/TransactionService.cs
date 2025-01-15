
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
                    .OrderByDescending(transaction => transaction.user_id)
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

        public override async Task<List<Transaction>> GetSortedTransaction(int userId, Dto.SortFormDTO sortForm)
        {
            var sortTransancation = _connection.Table<Transaction>()
                .Where(transaction => transaction.user_id == userId);

            if (!string.IsNullOrEmpty(sortForm.SearchTerm) ) {

                var searchTerm = sortForm.SearchTerm.ToLower();
                bool isNumeric = decimal.TryParse(sortForm.SearchTerm, out decimal searchAmount);


                if (isNumeric)
                {

                    sortTransancation = sortTransancation.Where(transaction =>
                    transaction.amount == searchAmount);


                }
                else
                {
                        sortTransancation = sortTransancation.Where(transaction =>
                        transaction.source.ToLower().Contains(searchTerm) ||
                        transaction.type.ToLower().Contains(searchTerm) ||
                        transaction.tagname.ToLower().Contains(searchTerm) ||
                        transaction.note.ToLower().Contains(searchTerm) ||
                        transaction.status.ToLower().Contains(searchTerm)
                    );
                }




            }
            switch (sortForm.SortBy)
            {
                case "Date":
                    if (sortForm.SortOrder == "High")
                    {
                        sortTransancation = sortTransancation.OrderByDescending(trans =>  trans.due_at);
                    }
                    else
                    {
                        sortTransancation = sortTransancation.OrderBy(t => t.due_at);
                    }
                    break;

                case "Amount":
                    if (sortForm.SortOrder == "High")
                    {
                        sortTransancation = sortTransancation.OrderByDescending(trans => trans.amount);
                    }
                    else
                    {
                        sortTransancation = sortTransancation.OrderBy(trans => trans.amount);
                    }
                    break;

                case "Debit":
                    sortTransancation = sortTransancation.Where(trans => trans.type.ToLower() == "debit");
                    if (sortForm.SortOrder == "High")
                    {
                        sortTransancation = sortTransancation.OrderByDescending(trans  => trans.amount);
                    }
                    else
                    {
                        sortTransancation = sortTransancation.OrderBy(trans => trans.amount);
                    }
                    break;

                case "Credit":
                    sortTransancation = sortTransancation.Where(trans => trans.type.ToLower() == "credit");
                    if (sortForm.SortOrder == "High")
                    {
                        sortTransancation = sortTransancation.OrderByDescending(trans => trans.amount);
                    }
                    else
                    {
                        sortTransancation = sortTransancation.OrderBy(trans => trans.amount);
                    }
                    break;

                case "Tag":
                    if (!string.IsNullOrEmpty(sortForm.TagName))
                    {
                        sortTransancation = sortTransancation.Where(trans => trans.tagname.ToLower() == sortForm.TagName.ToLower());
                    }
                    break;

                default:
                    sortTransancation = sortTransancation.OrderByDescending(t => t.created_at);
                    break;
            }

            return await sortTransancation.ToListAsync();

        }

        public override async Task UpdateTransaction(Transaction transaction, Guid transid)
        {

            var existingTransaction = await _connection.Table<Transaction>()
        .FirstOrDefaultAsync(trans => trans.id == transid);

            if (existingTransaction != null)
            {
                existingTransaction.source = transaction.source;
                existingTransaction.amount = transaction.amount;
                existingTransaction.due_at = transaction.due_at;
                existingTransaction.note = transaction.note;
                existingTransaction.status = transaction.status;
                existingTransaction.type = transaction.type;
                existingTransaction.tagname = transaction.tagname;

                await _connection.UpdateAsync(existingTransaction);
            }


        }
    }
}
