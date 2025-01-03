using BytesTracker.Repository;
using SQLite;

namespace BytesTracker.Database
{
    public class DatabaseService : DatabaseServiceRepo
    {

        private const string DB_Name = "projet_local";
        private readonly SQLiteAsyncConnection _connection;

        public DatabaseService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_Name));

            _connection.CreateTableAsync<Users>();
            _connection.CreateTableAsync<Tags>();





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

        public async Task Add_Tag(Tags tags)
        {
            await _connection.InsertAsync(tags);
        }



        public async Task<List<Tags>> Get_Tag(int userID)
        {
            try
            {

                var tags = await _connection.Table<Tags>()
                                 .Where(tag => tag.user_id == userID)
                                 .ToListAsync();

                return tags;
            }
            catch (Exception e)
            {

                throw new Exception("Error fetching tags: " + e.Message);

            }

        }


        public async Task Delete_Tag(int tagID, int userID)
        {

            await _connection.Table<Tags>()
                    .Where(t => t.id == tagID && t.user_id == userID)
                    .DeleteAsync();

        }

    }
}
