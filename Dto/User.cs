using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesTracker.Dto
{
    public class User
    {

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;

    }
}
