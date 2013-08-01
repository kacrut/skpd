using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class RequestInTransport
    {
        public int RequestInTransportID { get; set; }
        public int RequestID { get; set; }
        public int TransportID { get; set; }
        public virtual Request Request { get; set; }
        public virtual Transport Transport { get; set; }
    }
}
