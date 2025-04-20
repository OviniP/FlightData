using FlightData.Entities;
using FlightData.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightData.Services
{
    public class FlightSequenceValidator : IFlightSequenceValidator
    {
        public IList<Flight> Validate(IEnumerable<Flight> flights)
        {
            var inconsistencies = new List<Flight>();
            var flightsByAircraft = flights.GroupBy(f => f.FlightNumber);

            foreach (var group in flightsByAircraft)
            {
                var sorted = group.OrderBy(f => f.DepartureDateTime).ThenBy(f => f.ArrivalDateTime).ToList();

                for (int index = 0; index < sorted.Count; index++)
                {
                    var prev = index == 0 ? null : sorted[index - 1];
                    var curr = sorted[index];

                    if (IsArrivedBeforeDepart(curr))
                    {
                        inconsistencies.Add(curr);
                        continue;
                    }
                    if (IsDeparturedBeforePreviousArrival(prev, curr))
                    {
                        inconsistencies.Add(curr);
                        continue;
                    }
                    if (IsLocationInconsistant(prev, curr))
                    {
                        inconsistencies.Add(curr);
                        continue;
                    }
                }
            }

            return inconsistencies;
        }

        private bool IsArrivedBeforeDepart(Flight flight) {
            return (flight.DepartureTimeCasted > flight.ArrivalTimeCasted);
        }

        private bool IsDeparturedBeforePreviousArrival(Flight? previous, Flight current)
        {
            return (previous != null && previous.ArrivalTimeCasted > current.DepartureTimeCasted);
        }

        private bool IsLocationInconsistant(Flight? previous, Flight current)
        {
            return (previous != null && previous.ArrivalAirport == current.DepartureAirport);
        }
    }
}
