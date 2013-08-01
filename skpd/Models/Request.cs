using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace skpd.Models
{
    public partial class Request
    {
        public Request()
        {
            this.RequestInPrograms = new List<RequestInProgram>();
            this.RequestInTransports = new List<RequestInTransport>();
            this.RequestLogs = new List<RequestLog>();
            this.RequestReleases = new List<RequestRelease>();
        }

        public int RequestID { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime StartDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
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
        public virtual User User { get; set; }
        public virtual Country Country { get; set; }
        public virtual Country Country1 { get; set; }
        public virtual Position Position { get; set; }
        public virtual Position Position1 { get; set; }
        public virtual Flag Flag { get; set; }
        public virtual ICollection<RequestInProgram> RequestInPrograms { get; set; }
        public virtual ICollection<RequestInTransport> RequestInTransports { get; set; }
        public virtual ICollection<RequestLog> RequestLogs { get; set; }
        public virtual ICollection<RequestRelease> RequestReleases { get; set; }
    }
}
