using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace skpd.DTO
{
    public class RequestInProgramDTO
    {
        [Required]
        public int RequestID { get; set; }
        public int sendirilain { get; set; }
        [Required]
        public int UnitID { get; set; }
        [Required]
        public int ProgramID { get; set; }
        //public int ApproverPositionID { get; set; }
    }
}