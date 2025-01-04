using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesTracker.Dto
{
    public class Transaction
    {
        public string Sources { get; set; }
        public int Value { get; set; }
        public DateTime? DueDate { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int Tag { get; set; }
    }
}
