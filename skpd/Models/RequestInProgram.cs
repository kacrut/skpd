using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class RequestInProgram
    {
        public int RequestInProgramID { get; set; }
        public int RequestID { get; set; }
        public int ProgramID { get; set; }
        public virtual Program Program { get; set; }
        public virtual Request Request { get; set; }
    }
}
