using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class ExchangeRate
    {
        public int ExchangeRateID { get; set; }
        public int CurrencyID { get; set; }
        public decimal ExchangeRate1 { get; set; }
        public System.DateTime ExchangeDate { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
