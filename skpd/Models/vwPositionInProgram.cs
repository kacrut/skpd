using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class vwPositionInProgram
    {
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public int PositionID { get; set; }
        public string fullname { get; set; }
        public bool FLag { get; set; }
    }
}
