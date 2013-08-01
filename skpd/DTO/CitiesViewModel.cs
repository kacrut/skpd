using System.Collections.Generic;
using skpd.Models;

namespace skpd.DTO
{
	public class CitiesViewModel {
		public bool WasPosted { get; set; }
		public IEnumerable<City> AvailableCities { get; set; }
		public IEnumerable<City> SelectedCities { get; set; }
		public PostedCities PostedCities { get; set; }
	}
	public class PostedCities {
		// this array will be used to POST values from the form to the controller
		public string[] CityIDs { get; set; }
	}
}