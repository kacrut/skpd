using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class Region
    {
        public Region()
        {
            this.CountryInRegions = new List<CountryInRegion>();
        }

        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public virtual ICollection<CountryInRegion> CountryInRegions { get; set; }
    }
}
