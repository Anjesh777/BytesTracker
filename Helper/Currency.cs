using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesTracker.Helper
{
    public class Currency
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }


        public static List<Currency> GetAllCurrencies()
        {
            return new List<Currency>
            {
                new Currency { Code = "USD", Name = "US Dollar", Symbol = "$" },
                new Currency { Code = "EUR", Name = "Euro", Symbol = "€" },
                new Currency { Code = "GBP", Name = "British Pound", Symbol = "£" },
                new Currency { Code = "JPY", Name = "Japanese Yen", Symbol = "¥" },
                new Currency { Code = "AUD", Name = "Australian Dollar", Symbol = "$" },
                new Currency { Code = "CAD", Name = "Canadian Dollar", Symbol = "$" },
                new Currency { Code = "CHF", Name = "Swiss Franc", Symbol = "Fr" },
                new Currency { Code = "CNY", Name = "Chinese Yuan", Symbol = "¥" },
                new Currency { Code = "INR", Name = "Indian Rupee", Symbol = "₹" },
                new Currency { Code = "NZD", Name = "New Zealand Dollar", Symbol = "$" },
                new Currency { Code = "SGD", Name = "Singapore Dollar", Symbol = "$" },
                new Currency { Code = "HKD", Name = "Hong Kong Dollar", Symbol = "$" },
                new Currency { Code = "KRW", Name = "South Korean Won", Symbol = "₩" },
                new Currency { Code = "MXN", Name = "Mexican Peso", Symbol = "$" },
                new Currency { Code = "BRL", Name = "Brazilian Real", Symbol = "R$" },
                new Currency { Code = "ZAR", Name = "South African Rand", Symbol = "R" },
                new Currency { Code = "AED", Name = "UAE Dirham", Symbol = "د.إ" },
                new Currency { Code = "SAR", Name = "Saudi Riyal", Symbol = "﷼" },
                new Currency { Code = "RUB", Name = "Russian Ruble", Symbol = "₽" },
                new Currency { Code = "SEK", Name = "Swedish Krona", Symbol = "kr" }
            };
        }

        public static Currency GetCurrencyData(string code) {

            return GetAllCurrencies().FirstOrDefault(c => c.Code == code.ToUpper());
        }

    }

}
