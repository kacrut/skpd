using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class User
    {
        public User()
        {
            this.Requests = new List<Request>();
            this.RequestLogs = new List<RequestLog>();
            this.webpages_UsersInRoles = new List<webpages_UsersInRoles>();
            this.webpages_Membership = new List<webpages_Membership>();
        }

        public int ID { get; set; }
        public string NIK { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Rekening { get; set; }
        public string Email { get; set; }
        public int PositionID { get; set; }
        public string Npwp { get; set; }
        public string Alamat { get; set; }
        public string FaceUrl { get; set; }
        public bool isActive { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<RequestLog> RequestLogs { get; set; }
        public virtual Position Position { get; set; }
        public virtual ICollection<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
        public virtual ICollection<webpages_Membership> webpages_Membership { get; set; }
    }
}
