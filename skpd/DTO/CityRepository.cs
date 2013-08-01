﻿using System.Collections.Generic;
using System.Linq;

namespace skpd.DTO
{
	public static class CityRepository {
		
        public static City Get(int id) {
			return GetAll().SingleOrDefault(x => x.Id.Equals(id));
		}
		public static IEnumerable<City> GetAll() {
			return new List<City> {
			                      	new City {Name = "Monroe", Id = 1, Tags = new {what = "smallCity"}},
			                      	new City {Name = "Moscow", Id = 2, Tags = new {what = "bigCity"}},
			                      	new City {Name = "New Orleans", Id = 3, Tags = ""},
			                      	new City {Name = "Ottawa", Id = 4, Tags = ""},
			                      	new City {Name = "Mumbai", Id = 5, Tags = ""},
			                      	new City {Name = "Rome", Id = 6, Tags = ""},
			                      	new City {Name = "Rio", Id = 7, Tags = ""}
			                      };
		}
	}
}