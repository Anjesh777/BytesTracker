using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesTracker.Dto
{
    public  class SortFormDTO
    {
        public string SearchTerm { get; set; } = string.Empty;
        public string SortBy { get; set; } = "Sort by";
        public string SortOrder { get; set; } = "High";
        public string TagName { get; set; } = string.Empty;
        public bool showHighLow { get; set; }
        public bool showTags { get; set; }

    }
}
