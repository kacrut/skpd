using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class Country
    {
        public Country()
        {
            this.CashDailies = new List<CashDaily>();
            this.CountryInRegions = new List<CountryInRegion>();
            this.Positions = new List<Position>();
            this.Requests = new List<Request>();
            this.Requests1 = new List<Request>();
            this.RequestLogs = new List<RequestLog>();
            this.RequestLogs1 = new List<RequestLog>();
        }

        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int CountryAreaID { get; set; }
        public virtual ICollection<CashDaily> CashDailies { get; set; }
        public virtual CountryArea CountryArea { get; set; }
        public virtual ICollection<CountryInRegion> CountryInRegions { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Request> Requests1 { get; set; }
        public virtual ICollection<RequestLog> RequestLogs { get; set; }
        public virtual ICollection<RequestLog> RequestLogs1 { get; set; }
    }
}
