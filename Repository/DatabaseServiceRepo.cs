using BytesTracker.Database;


namespace BytesTracker.Repository
{
    public interface DatabaseServiceRepo
    {

        Task<bool> IsUserRegistered(string username);
        Task Create_User(Users users);
        Task<bool> Login_User(string userName, string password);
        Task<int> Get_UserID(string userName);
        Task Add_Tag(Tags tags);
        Task<List<Tags>> Get_Tag(int userId);
        Task Delete_Tag(int tagId, int userId);


    }
}
