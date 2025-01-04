using BytesTracker.Model;

namespace BytesTracker.Repository
{
    public abstract class UserRepo
    {
       

        public abstract Task<bool> Login_User(string UserName, string Password);
        public abstract Task Create_User(Users users);
        public abstract Task<int> Get_UserID(string userName);
        public abstract Task<bool> IsUserRegistered(string username);
        public abstract Task<int> GetUserIdByUsername(string username);

    }
}
