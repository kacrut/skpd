using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace skpd.DTO
{
    public class RangeReleaseReport
    {
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime From { get; set; }
    }
}