using BytesTracker.Database;

namespace BytesTracker.Repository
{
    public abstract class LoginRepo
    {
        protected readonly DatabaseService DbService;

        protected LoginRepo(DatabaseService dbService)
        {
            DbService = dbService;
        }

        public abstract Task<bool> Login_User(String UserName, String Password);



    }
}
