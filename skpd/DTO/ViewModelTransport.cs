using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skpd.DTO
{
    public class ViewModelTransport
    {
        public bool WasPosted { get; set; }
        public IEnumerable<Trans> AvailableCities { get; set; }
        public IEnumerable<Trans> SelectedCities { get; set; }
        public PostedCities PostedCities { get; set; }
    }
    public class Trans 
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}