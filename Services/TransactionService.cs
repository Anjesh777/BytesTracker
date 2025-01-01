using BytesTracker.Database;
using BytesTracker.Repository;

namespace BytesTracker.Services
{
    public class TransactionService : TransationRepo
    {
        public TransactionService(DatabaseService dbService) : base(dbService)
        {
        }

        public override async Task<List<Tags>> GetUserTags(int userId)
        {
            return await DbService.Get_Tag(userId);
        }

        public override async Task AddTag(Tags tag)
        {
            await DbService.Add_Tag(tag);
        }

        public override async Task<int> GetUserId(string username)
        {
            return await DbService.Get_UserID(username);
        }

        public override async Task<int> GetUserIdByUsername(string username)
        {
            return await DbService.Get_UserID(username);
        }

        public override async Task<List<string>> GetByUserTags(int userID)
        {
            var tags = await DbService.Get_Tag(userID);
            return tags.Select(tag => tag.TagName).ToList();

        }

        public override async Task DeleteTag(int tagID, int userID)
        {
            await DbService.Delete_Tag(tagID, userID); 
        }
    }
}