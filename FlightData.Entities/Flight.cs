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

        private DateTime? _departureTimeCasted;
        public DateTime? DepartureTimeCasted
        {
            get
            {
                if (_departureTimeCasted == null && DateTime.TryParse(DepartureDateTime, out var parsed))
                {
                    _departureTimeCasted = parsed;
                }
                return _departureTimeCasted;
            }
            private set => _departureTimeCasted = value;
        }

        private DateTime? _arrivalTimeCasted;
        public DateTime? ArrivalTimeCasted
        {
            get
            {
                if (_arrivalTimeCasted == null && DateTime.TryParse(ArrivalDateTime, out var parsed))
                {
                    _arrivalTimeCasted = parsed;
                }
                return _arrivalTimeCasted;
            }
            private set => _arrivalTimeCasted = value;
        }
    }
}
