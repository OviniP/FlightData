using FlightData.Entities;
using FlightData.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightData.Services
{
    public class FlightDataService: IFlightDataService
    {
        private readonly IDataReaderService _dataReaderService;
        private readonly IFlightSequenceValidator _flightSequenceValidator;
        public FlightDataService(IDataReaderService daaReaderService, IFlightSequenceValidator flightSequenceValidator) { 
            _dataReaderService = daaReaderService;
            _flightSequenceValidator = flightSequenceValidator;
        }

        public async Task<IList<Flight>> GetFlightDataAsync() { 
            var flightData = await _dataReaderService.GetFlights();
            return flightData;
        }

        public async Task<IList<Flight>> GetFlightInconsistenciesAsync()
        {
            var flightData = await _dataReaderService.GetFlights();
            var inconsistancies =  _flightSequenceValidator.Validate(flightData);
            return inconsistancies;
        }
    }
}
