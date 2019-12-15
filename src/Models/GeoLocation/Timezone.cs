namespace Models.GeoLocation
{
    public class Timezone
    {
        public string timezone_identifier { get; set; }
        public string timezone_abbr { get; set; }
        public int utc_offset_sec { get; set; }
        public string is_dst { get; set; }
    }
}