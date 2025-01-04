using BytesTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesTracker.Repository
{
    public abstract class TagRepo
    {

       

        public abstract Task<List<Tags>> GetUserTags(int userId);
        public abstract Task AddTag(Tags tag);
        public abstract Task DeleteTag(int tagID, int userID);

       // public abstract Task<List<String>> GetUserTagNames(int userId);







    }


}

