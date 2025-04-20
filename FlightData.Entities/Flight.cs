namespace FlightData.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? AircraftType { get; set; }
        public string? FlightNumber { get; set; }
        public string? DepartureAirport { get; set; }
        public string? DepartureDateTime { get; set; }
        public string? ArrivalAirport { get; set; }
        public string? ArrivalDateTime { get; set; }

        public DateTime? DepartureTimeCasted {
            get
            {
                if (DateTime.TryParse(DepartureDateTime, out var parsed))
                {
                    return parsed;
                }
                return null;
            }
        }

        public DateTime? ArrivalTimeCasted
        {
            get
            {
                if (DateTime.TryParse(ArrivalDateTime, out var parsed))
                {
                    return parsed;
                }
                return null;
            }
        }
    }
}
