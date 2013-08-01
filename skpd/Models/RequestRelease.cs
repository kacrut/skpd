using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace skpd.Models
{
    public partial class RequestRelease
    {
        public int RequestReleaseID { get; set; }
        public int RequestID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int PositionCountryID { get; set; }
        public string PositionCountryName { get; set; }
        public int PositionTypeID { get; set; }
        public string PositionTypeName { get; set; }
        public int LevelPositionID { get; set; }
        public string LevelPositionName { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public string Rekening { get; set; }
        public string Email { get; set; }
        public string NIK { get; set; }
        public string Npwp { get; set; }
        public string Alamat { get; set; }
        public Nullable<int> ToCountryAreaID { get; set; }
        public string ToCountryAreaName { get; set; }
        public Nullable<int> ToRegionID { get; set; }
        public string ToRegionName { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string ToCountryName { get; set; }
        public Nullable<int> FromCountryAreaID { get; set; }
        public string FromCountryAreaName { get; set; }
        public Nullable<int> FromRegionID { get; set; }
        public string FromRegionName { get; set; }
        public Nullable<int> FromCountryID { get; set; }
        public string FromCountryName { get; set; }
        public string EventName { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime StartDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime EndDate { get; set; }
        public int Days { get; set; }
        public string CurrencyName { get; set; }
        public decimal ExchangeRate { get; set; }
        public System.DateTime ExchangeDate { get; set; }
        public string CashStayOrCashNotStay { get; set; }
        public decimal CashPerDay { get; set; }
        public decimal ConvertCashPerDay { get; set; }
        public decimal TotalCash { get; set; }
        public string ProgramID { get; set; }
        public string ProgramName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int FlagID { get; set; }
        public System.DateTime FlagCreatedDate { get; set; }
        public Nullable<int> SignID { get; set; }
        public string SignName { get; set; }
        public virtual Request Request { get; set; }
    }
}
