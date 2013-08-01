using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class Position
    {
        public Position()
        {
            this.Users = new List<User>();
            this.Position1 = new List<Position>();
            this.Position11 = new List<Position>();
            this.Requests = new List<Request>();
            this.Requests1 = new List<Request>();
            this.RequestLogs = new List<RequestLog>();
            this.RequestLogs1 = new List<RequestLog>();
        }

        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public int UnitID { get; set; }
        public int CountryID { get; set; }
        public int AssignID { get; set; }
        public int PositionTypeID { get; set; }
        public Nullable<int> SignID { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Position> Position1 { get; set; }
        public virtual Position Position2 { get; set; }
        public virtual PositionType PositionType { get; set; }
        public virtual ICollection<Position> Position11 { get; set; }
        public virtual Position Position3 { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Request> Requests1 { get; set; }
        public virtual ICollection<RequestLog> RequestLogs { get; set; }
        public virtual ICollection<RequestLog> RequestLogs1 { get; set; }
    }
}
