using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class CountryInRegion
    {
        public int CountryInRegionID { get; set; }
        public int RegionID { get; set; }
        public int CountryID { get; set; }
        public virtual Country Country { get; set; }
        public virtual Region Region { get; set; }
    }
}
