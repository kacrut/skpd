using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace skpd.DTO
{
    public class RequestDTO
    {
        //[Key]
        //public int RequestID { get; set; }
        //[Required]
        //[DataType(DataType.Date)]
        //[DisplayName("Dari")]
        //public DateTime StartDate { get; set; }
        //[Required]
        //[DataType(DataType.Date)]
        //[DisplayName("Sampai")]
        //public DateTime EndDate { get; set; }
        //[Required]
        //public string Destination { get; set; }
        //[Required]
        //public string EventName { get; set; }
        //public int PositionID { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public int Flag { get; set; }
        //public Nullable<System.DateTime> ApprovedDate { get; set; }
        //public int ApprovalPositionID { get; set; }
        //public Nullable<System.DateTime> RejectedDate { get; set; }
        //public string RejectedReason { get; set; }
        //public ViewModelTransport ViewModelTransport { get; set; }
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
        public int sendirilain { get; set; }
        public int DivisionID { get; set; }
        public int ProgramID { get; set; }
    }
}