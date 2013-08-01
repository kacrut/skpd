using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class vwUserProfile
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string fullname { get; set; }
        public string Email { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public int PositionTypeID { get; set; }
        public string PositionTypeName { get; set; }
        public int LevelPositionID { get; set; }
        public string LevelPositionName { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int AssignID { get; set; }
        public string Assign { get; set; }
        public int SignID { get; set; }
        public string SignName { get; set; }
        public string FaceUrl { get; set; }
    }
}
