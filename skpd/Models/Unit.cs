using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class Unit
    {
        public Unit()
        {
            this.Positions = new List<Position>();
        }

        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public int DivisionID { get; set; }
        public virtual Division Division { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
