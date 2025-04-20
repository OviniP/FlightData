using FlightData.Entities;
using FlightData.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace FlightData.Services
{
    /// <summary>
    /// Validates the logical sequence and consistency of a collection of flight records.
    /// </summary>
    public class FlightSequenceValidator : IFlightSequenceValidator
    {
        /// <summary>
        /// Validates a sequence of flights for logical inconsistencies such as incorrect timing or airport mismatches.
        /// </summary>
        /// <param name="flights">A collection of <see cref="Flight"/> records to validate.</param>
        /// <returns>
        /// A list of <see cref="Flight"/> objects that contain inconsistencies.
        /// </returns>
        public IList<Flight> Validate(IEnumerable<Flight> flights)
        {
            var inconsistencies = new List<Flight>();
            var flightsByAircraft = flights.GroupBy(f => f.FlightNumber);

            foreach (var group in flightsByAircraft)
            {
                var sorted = group.OrderBy(f => f.DepartureDateTime)
                                  .ThenBy(f => f.ArrivalDateTime)
                                  .ToList();

                for (int index = 0; index < sorted.Count; index++)
                {
                    var prev = index == 0 ? null : sorted[index - 1];
                    var curr = sorted[index];

                    if (IsArrivedBeforeDepart(curr) ||
                        IsDeparturedBeforePreviousArrival(prev, curr) ||
                        IsLocationInconsistant(prev, curr))
                    {
                        inconsistencies.Add(curr);
                    }
                }
            }

            return inconsistencies;
        }

        /// <summary>
        /// Checks if a flight arrives before it departs.
        /// </summary>
        /// <param name="flight">The flight to check.</param>
        /// <returns>True if the arrival time is earlier than the departure time.</returns>
        private bool IsArrivedBeforeDepart(Flight flight)
        {
            return flight.DepartureTimeCasted > flight.ArrivalTimeCasted;
        }

        /// <summary>
        /// Checks if a flight departs before the previous journery ends.
        /// </summary>
        /// <param name="previous">The previous flight in sequence.</param>
        /// <param name="current">The current flight.</param>
        /// <returns>True if the current flight departs before the previous one arrives.</returns>
        private bool IsDeparturedBeforePreviousArrival(Flight? previous, Flight current)
        {
            return previous != null && previous.ArrivalTimeCasted > current.DepartureTimeCasted;
        }

        /// <summary>
        /// Checks if the arrival airport of the previous flight is inconsistent with the departure airport of the current flight.
        /// </summary>
        /// <param name="previous">The previous flight.</param>
        /// <param name="current">The current flight.</param>
        /// <returns>True if the airport locations are inconsistent.</returns>
        private bool IsLocationInconsistant(Flight? previous, Flight current)
        {
            return previous != null && previous.ArrivalAirport == current.DepartureAirport;
        }
    }
}
