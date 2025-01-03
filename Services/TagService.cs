using BytesTracker.Repository;
using BytesTracker.Model;
using SQLite;


namespace BytesTracker.Services
{

    public class TagService : TagRepo
        {
        private readonly SQLiteAsyncConnection _connection;

        public TagService(SQLiteAsyncConnection connection) 
        {
            _connection = connection;
        }

        public override async Task AddTag(Tags tags)
            {
                await _connection.InsertAsync(tags);
            }

            public override async Task<List<Tags>> GetUserTags(int userID)
            {
                try
                {
                    return await _connection.Table<Tags>()
                        .Where(tag => tag.user_id == userID)
                        .ToListAsync();
                }
                catch (Exception e)
                {
                    throw new Exception("Error fetching tags: " + e.Message);
                }
            }

            public override async Task DeleteTag(int tagID, int userID)
            {
                await _connection.Table<Tags>()
                    .Where(t => t.id == tagID && t.user_id == userID)
                    .DeleteAsync();
            }

       
    }
    }

