using BytesTracker.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesTracker.Services
{
    public class UserService
    {
        private readonly SQLiteAsyncConnection _connection;
        public UserService(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> IsUserRegistered(string username)
        {
            var user = await _connection.Table<Users>().FirstOrDefaultAsync(u => u.UserName == username);
            return user != null;
        }

        public async Task Create_User(Users users)
        {
            await _connection.InsertAsync(users);
        }

        public async Task<bool> Login_User(String UserName, String Password)
        {
            try
            {
                var user = await _connection.Table<Users>()
                    .Where(x => x.UserName == UserName && x.Password == Password)
                    .FirstOrDefaultAsync();
                return user != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> Get_UserID(String userName)
        {
            try
            {
                var user = await _connection.Table<Users>().FirstOrDefaultAsync(x => x.UserName == userName);
                return user?.Id ?? -1;
            }
            catch
            {
                return -1;
            }
        }
    }



}
}
