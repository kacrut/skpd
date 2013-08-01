using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace skpd.DTO
{
    public class RejectRequest
    {
        [Required]
        public int RequestID { get; set; }
        [Required]
        public string RejectedReason { get; set; }
    }
}