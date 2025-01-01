using BytesTracker.Repository;
using BytesTracker.Database;

namespace BytesTracker.Services
{
    public class LoginService : LoginRepo
    {
        public LoginService(DatabaseService dbService) : base(dbService)
        {
        }

        public override async Task<bool> Login_User(string UserName, string Password)
        {
            return await DbService.Login_User(UserName,Password);
        }
    }
}
