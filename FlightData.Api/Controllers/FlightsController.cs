using FlightData.Entities;
using FlightData.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightData.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightDataService _flightDataService;
        public FlightsController(IFlightDataService flightDataService)
        {
                _flightDataService = flightDataService;
        }

        [HttpGet]
        public  async Task<ActionResult<IList<Flight>>> Get()
        {
            var flightData = await _flightDataService.GetFlightDataAsync();
            return Ok(flightData);
        }

        [HttpGet]
        public async Task<ActionResult<IList<Flight>>> GetInconsistencies()
        {
            var flightData = await _flightDataService.GetFlightInconsistenciesAsync();
            return Ok(flightData);
        }
    }
}
