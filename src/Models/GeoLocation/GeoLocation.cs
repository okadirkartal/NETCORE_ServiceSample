using System.Collections.Generic;

namespace Models.GeoLocation
{
    public class GeoLocation
    {
        public string zip_code { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public Timezone timezone { get; set; }
        public List<AcceptableCityName> acceptable_city_names { get; set; }
        public List<int> area_codes { get; set; }
    }
}