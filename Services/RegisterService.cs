using BytesTracker.Repository;
using BytesTracker.Database;

namespace BytesTracker.Services
{
    public class RegisterService : RegisterRepo
    {
        public RegisterService(DatabaseService dbService) : base(dbService)
        {
        }

        public override async Task Create_User(Users users)
        {
            await DbService.Create_User(users);
        }
    }
}
