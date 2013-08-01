using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class Flag
    {
        public Flag()
        {
            this.Requests = new List<Request>();
            this.RequestLogs = new List<RequestLog>();
        }

        public int FlagID { get; set; }
        public string FlagName { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<RequestLog> RequestLogs { get; set; }
    }
}
