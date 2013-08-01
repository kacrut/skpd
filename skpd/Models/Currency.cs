using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class Currency
    {
        public Currency()
        {
            this.CashDailies = new List<CashDaily>();
            this.ExchangeRates = new List<ExchangeRate>();
        }

        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public virtual ICollection<CashDaily> CashDailies { get; set; }
        public virtual ICollection<ExchangeRate> ExchangeRates { get; set; }
    }
}
