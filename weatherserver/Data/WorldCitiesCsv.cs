namespace weatherserver.Data
{
    public class WorldCitiesCsv
    {
        public string City { get; set; } = null!;
        public string City_ascii { get; set; } = null!;
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string Country { get; set; } = null!;
        public string Iso2 { get; set; } = null!;
        public string Iso3 { get; set; } = null!;
        public decimal? Population { get; set; }
        public long Id { get; set; }
    }
}
