using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class Program
    {
        public Program()
        {
            this.RequestInPrograms = new List<RequestInProgram>();
        }

        public int ProgramID { get; set; }
        public int PositionID { get; set; }
        public string ProgramName { get; set; }
        public bool FLag { get; set; }
        public virtual ICollection<RequestInProgram> RequestInPrograms { get; set; }
    }
}
