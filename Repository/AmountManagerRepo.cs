using BytesTracker.Database;


namespace BytesTracker.Repository
{
    public abstract class AmountManagerRepo
    {
        protected readonly DatabaseService DbService;

        protected AmountManagerRepo(DatabaseService dbService)
        {
            DbService = dbService;
        }



    }
}
