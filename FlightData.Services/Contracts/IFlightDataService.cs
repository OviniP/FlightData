using FlightData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightData.Services.Contracts
{
    public interface IFlightDataService
    {
        Task<IList<Flight>> GetFlightDataAsync();
        Task<IList<Flight>> GetFlightInconsistenciesAsync();
    }
}
