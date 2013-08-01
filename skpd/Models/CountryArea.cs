using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class CountryArea
    {
        public CountryArea()
        {
            this.Countries = new List<Country>();
        }

        public int CountryAreaID { get; set; }
        public string CountryAreaName { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
    }
}
