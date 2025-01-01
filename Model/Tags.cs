using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesTracker.Model
{
    public class Tags
    {

        public int UserId { get; set; }

        public string TagName { get; set; } = string.Empty;
        public string TagDescription { get; set; } = string.Empty;

    }
}
