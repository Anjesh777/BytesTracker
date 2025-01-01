using BytesTracker.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesTracker.Repository
{
    public abstract class TransationRepo
    {

        protected readonly DatabaseService DbService;

        protected TransationRepo(DatabaseService dbService)
        {
            DbService = dbService;
        }

        public abstract Task<List<Tags>> GetUserTags(int userId);
        public abstract Task AddTag(Tags tag);
        public abstract Task<int> GetUserId(string username);
        public abstract Task<int> GetUserIdByUsername(string username);
        public abstract Task<List<String>> GetByUserTags(int userID);
        public abstract Task DeleteTag(int tagID, int userID);








    }


}

