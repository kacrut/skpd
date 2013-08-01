using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class Transport
    {
        public Transport()
        {
            this.RequestInTransports = new List<RequestInTransport>();
        }

        public int TransportID { get; set; }
        public string TransportName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RequestInTransport> RequestInTransports { get; set; }
    }
}
