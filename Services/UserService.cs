using BytesTracker.Repository;
using BytesTracker.Model;
using SQLite;

namespace BytesTracker.Services
{
    public class UserService : UserRepo
    {
        private readonly SQLiteAsyncConnection _connection;

        public UserService(SQLiteAsyncConnection connection) 
        {
            _connection = connection;
        }

        public override async Task<bool> IsUserRegistered(string username)
        {
            var user = await _connection.Table<Users>().FirstOrDefaultAsync(u => u.UserName == username);
            return user != null;
        }

        public override async Task Create_User(Users users)
        {
            await _connection.InsertAsync(users);
        }

        public override async Task<bool> Login_User(string UserName, string Password)
        {
            try
            {
                var user = await _connection.Table<Users>()
                    .Where(x => x.UserName == UserName && x.Password == Password)
                    .FirstOrDefaultAsync();
                return user != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                return false;
            }
        }

        public override async Task<int> Get_UserID(string userName)
        {
            try
            {
                var user = await _connection.Table<Users>().FirstOrDefaultAsync(x => x.UserName == userName);
                return user?.Id ?? -1; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user ID: {ex.Message}");
                return -1;
            }
        }

        public override async Task<int> GetUserIdByUsername(string username)
        {
            return await Get_UserID(username);
        }



    }
}
