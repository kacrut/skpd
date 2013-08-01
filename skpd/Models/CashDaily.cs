using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class CashDaily
    {
        public int CashDailyID { get; set; }
        public int CountryID { get; set; }
        public int LevelPositionID { get; set; }
        public decimal CashStay { get; set; }
        public decimal CashNotStay { get; set; }
        public int CurrencyID { get; set; }
        public virtual Country Country { get; set; }
        public virtual LevelPosition LevelPosition { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
