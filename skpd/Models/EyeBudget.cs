using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class EyeBudget
    {
        public int ProgramID { get; set; }
        public int PositionID { get; set; }
        public string ProgramName { get; set; }
        public bool FLag { get; set; }
    }
}
