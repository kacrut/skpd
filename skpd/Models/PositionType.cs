using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class PositionType
    {
        public PositionType()
        {
            this.Positions = new List<Position>();
        }

        public int PositionTypeID { get; set; }
        public string PositionTypeName { get; set; }
        public Nullable<int> LevelPositionID { get; set; }
        public virtual LevelPosition LevelPosition { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
