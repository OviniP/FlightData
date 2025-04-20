using FlightData.Entities;

namespace FlightData.Services.Contracts
{
    /// <summary>
    /// Defines a contract for validating the logical sequence of flight data.
    /// </summary>
    public interface IFlightSequenceValidator
    {
        /// <summary>
        /// Validates a collection of flight records to identify sequence-related inconsistencies.
        /// </summary>
        /// <param name="flights">A collection of <see cref="Flight"/> objects to validate.</param>
        /// <returns>
        /// A list of <see cref="Flight"/> objects that contain sequence inconsistencies 
        /// (e.g., overlapping flight times, incorrect airport sequences, etc.).
        /// </returns>
        IList<Flight> Validate(IEnumerable<Flight> flights);
    }
}
