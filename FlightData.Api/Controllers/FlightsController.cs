using FlightData.Entities;
using FlightData.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FlightData.Api.Controllers
{
    /// <summary>
    /// API Controller to work with Flight related data
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightDataService _flightDataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightsController"/> class.
        /// </summary>
        /// <param name="flightDataService">Service used to retrieve flight data.</param>
        public FlightsController(IFlightDataService flightDataService)
        {
                _flightDataService = flightDataService;
        }

        /// <summary>
        /// Retrieves a list of all available flight data.
        /// </summary>
        /// <returns>A list of <see cref="Flight"/> objects.</returns>
        [HttpGet]
        public  async Task<ActionResult<IList<Flight>>> Get()
        {
            var flightData = await _flightDataService.GetFlightDataAsync();
            return Ok(flightData);
        }

        /// <summary>
        /// Retrieves a list of flights with detected data inconsistencies.
        /// </summary>
        /// <returns>A list of <see cref="Flight"/> objects with inconsistencies.</returns>
        [HttpGet]
        public async Task<ActionResult<IList<Flight>>> GetInconsistencies()
        {
            var flightData = await _flightDataService.GetFlightInconsistenciesAsync();
            return Ok(flightData);
        }
    }
}
