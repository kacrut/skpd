using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class LevelPosition
    {
        public LevelPosition()
        {
            this.CashDailies = new List<CashDaily>();
            this.PositionTypes = new List<PositionType>();
        }

        public int LevelPositionID { get; set; }
        public string LevelPositionName { get; set; }
        public virtual ICollection<CashDaily> CashDailies { get; set; }
        public virtual ICollection<PositionType> PositionTypes { get; set; }
    }
}
