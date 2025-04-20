using CsvHelper.Configuration;

namespace FlightData.Entities
{
    public class FlightMap : ClassMap<Flight>
    {
        public FlightMap()
        {
            Map(f => f.Id).Name("id");
            Map(f => f.RegistrationNumber).Name("aircraft_registration_number");
            Map(f => f.AircraftType).Name("aircraft_type");
            Map(f => f.FlightNumber).Name("flight_number");
            Map(f => f.DepartureAirport).Name("departure_airport");
            Map(f => f.DepartureDateTime).Name("departure_datetime");
            Map(f => f.ArrivalAirport).Name("arrival_airport");
            Map(f => f.ArrivalDateTime).Name("arrival_datetime");
        }
    }

}
