using FlightData.Entities;

namespace FlightData.Services.Contracts
{
    /// <summary>
    /// Provides operations for retrieving and validating flight data.
    /// </summary>
    public interface IFlightDataService
    {
        /// <summary>
        /// Asynchronously retrieves all available flight data.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains a list of <see cref="Flight"/> objects.
        /// </returns>
        Task<IList<Flight>> GetFlightDataAsync();

        /// <summary>
        /// Asynchronously retrieves flight records that contain data inconsistencies.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains a list of <see cref="Flight"/> objects
        /// that have inconsistencies (e.g., missing or conflicting data).
        /// </returns>
        Task<IList<Flight>> GetFlightInconsistenciesAsync();
    }
}
