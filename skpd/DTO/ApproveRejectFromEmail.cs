using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace skpd.DTO
{
    public class ApproveRequestFromEmail
    {
        public int RequestID { get; set; }
        public string UserName { get; set; }
    }

    public class RejectRequestFromEmail
    {
        public int RequestID { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Reason { get; set; }
    }

    public class ApproveBudgetFromEmail
    {
        public int RequestID { get; set; }
        public string UserName { get; set; }
        public string BudgetID { get; set; }
        public string BudgetName { get; set; }
    }

    public class RejectBudgetFromEmail
    {
        public int RequestID { get; set; }
        public string UserName { get; set; }
        public string BudgetID { get; set; }
        public string BudgetName { get; set; }
        [Required]
        public string Reason { get; set; }
    }
}