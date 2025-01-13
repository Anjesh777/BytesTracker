using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Diagnostics.CodeAnalysis;


namespace BytesTracker.Model
{
    [Table("Transaction")]
    public class Transaction
    {

        [PrimaryKey]
        [AutoIncrement]
        [Column("transaction_id")]
        public Guid id { get; set; }
        [Column("user_id")]
        [ForeignKey(typeof(Users))]
        public int user_id { get; set; }
        [Column("source")]
        public string source { get; set; }
        [Column("Amount")]

        public int amount { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; } = DateTime.UtcNow.Date;
        [Column("due_at")]
        public DateTime due_at { get; set; }
        [Column("note")]

        public string note { get; set; }

        [Column("status")]

        public string status { get; set; } 

        [Column("type")]

        public string type { get; set; }
 

        [Column("tagname")]
        public string tagname { get; set; }
        



    }

}
