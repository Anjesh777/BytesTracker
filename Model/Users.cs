using SQLite;
using SQLiteNetExtensions.Attributes;


namespace BytesTracker.Model
{
    [Table("Users")]
    public class Users
    {

        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_name")]
        [Unique]
        public string UserName { get; set; }
  
        [Column("user_password")]
        public string Password { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]    
        public List<Tags> Tags { get; set; }

        [Column("amount")]

        public int amount { get; set; }

    }
}
