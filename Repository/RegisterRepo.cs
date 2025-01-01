using BytesTracker.Database;


namespace BytesTracker.Repository
{
    public abstract class RegisterRepo
    {

        protected readonly DatabaseService DbService;

        public RegisterRepo(DatabaseService dbService)
        {
            DbService = dbService;
        }

        public abstract Task Create_User(Users users);




    }
}
