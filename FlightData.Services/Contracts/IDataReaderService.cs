using FlightData.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightData.Services.Contracts
{
    /// <summary>
    /// Defines a contract for reading flight data from a data source.
    /// </summary>
    public interface IDataReaderService
    {
        /// <summary>
        /// Asynchronously retrieves a list of flights from the data source.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Flight"/> objects.</returns>
        Task<IList<Flight>> GetFlights();
    }
}
