using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class Division
    {
        public Division()
        {
            this.Units = new List<Unit>();
        }

        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
    }
}
