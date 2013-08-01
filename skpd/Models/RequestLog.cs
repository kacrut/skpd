using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class RequestLog
    {
        public int RequestLogID { get; set; }
        public int RequestID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int FromCountryID { get; set; }
        public int ToCountryID { get; set; }
        public string EventName { get; set; }
        public int PositionID { get; set; }
        public int UserID { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int FlagID { get; set; }
        public System.DateTime FlagCreatedDate { get; set; }
        public int ApprovalPositionID { get; set; }
        public string RejectedReason { get; set; }
        public System.DateTime RequestLogDate { get; set; }
        public virtual User User { get; set; }
        public virtual Country Country { get; set; }
        public virtual Country Country1 { get; set; }
        public virtual Position Position { get; set; }
        public virtual Position Position1 { get; set; }
        public virtual Flag Flag { get; set; }
        public virtual Request Request { get; set; }
    }
}
