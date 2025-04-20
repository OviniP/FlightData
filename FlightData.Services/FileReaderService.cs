using CsvHelper.Configuration;
using CsvHelper;
using FlightData.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightData.Services.Contracts;

namespace FlightData.Services
{
    public class FileReaderService : IDataReaderService
    {
        private readonly string _filePath;
        private IList<Flight>? _flights;

        public FileReaderService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<IList<Flight>> GetFlights() {
            _flights = _flights ?? await LoadData();
            return _flights; 
        }

        private async Task<IList<Flight>> LoadData()
        {
            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            });

            csv.Context.RegisterClassMap<FlightMap>();
            return await csv.GetRecordsAsync<Flight>().ToListAsync();
        }
    }
}
