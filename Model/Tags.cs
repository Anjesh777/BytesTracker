using SQLite;
using SQLiteNetExtensions.Attributes;


namespace BytesTracker.Model
{
    [Table("Tags")]
    public class Tags
    {


        [PrimaryKey]
        [AutoIncrement]
        [Column("tag_id")]
        public int id { get; set; }

        [Column("user_id")]

        [ForeignKey(typeof(Users))]

        public int user_id { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; } = DateTime.UtcNow.Date;

        [Column("tag_name")]
        public string TagName { get; set; }
        [Column("tag_description")]
        public string TagDescription { get; set; }

        [ManyToOne]
        public Users User { get; set; }
    }
}
