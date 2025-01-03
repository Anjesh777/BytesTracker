using SQLite;
using SQLiteNetExtensions.Attributes;


namespace BytesTracker.Model
{
    [Table("Transaction")]
    internal class Transaction
    {

        [PrimaryKey]
        [AutoIncrement]
        [Column("transaction_id")]
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
        [Column("Amount")]
        public int Amount { get; set; }



    }

}
