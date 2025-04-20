using FlightData.Entities;
using FlightData.Services.Contracts;

namespace FlightData.Services
{
    /// <summary>
    /// Provides flight data operations, including retrieving all flight records and detecting inconsistencies.
    /// </summary>
    public class FlightDataService : IFlightDataService
    {
        private readonly IDataReaderService _dataReaderService;
        private readonly IFlightSequenceValidator _flightSequenceValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightDataService"/> class.
        /// </summary>
        /// <param name="daaReaderService">The data reader service used to retrieve flight data.</param>
        /// <param name="flightSequenceValidator">The validator used to detect inconsistencies in flight sequences.</param>
        public FlightDataService(
            IDataReaderService daaReaderService,
            IFlightSequenceValidator flightSequenceValidator)
        {
            _dataReaderService = daaReaderService;
            _flightSequenceValidator = flightSequenceValidator;
        }

        /// <summary>
        /// Asynchronously retrieves all flight data.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains a list of all <see cref="Flight"/> records.
        /// </returns>
        public async Task<IList<Flight>> GetFlightDataAsync()
        {
            var flightData = await _dataReaderService.GetFlights();
            return flightData;
        }

        /// <summary>
        /// Asynchronously retrieves flight records that have sequence inconsistencies.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains a list of <see cref="Flight"/> records
        /// that contain logical errors or inconsistencies.
        /// </returns>
        public async Task<IList<Flight>> GetFlightInconsistenciesAsync()
        {
            var flightData = await _dataReaderService.GetFlights();
            var inconsistencies = _flightSequenceValidator.Validate(flightData);
            return inconsistencies;
        }
    }
}
