using CsvHelper.Configuration;
using CsvHelper;
using FlightData.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FlightData.Services.Contracts;

namespace FlightData.Services
{
    /// <summary>
    /// Service for reading flight data from a CSV file.
    /// </summary>
    public class FileReaderService : IDataReaderService
    {
        private readonly string _filePath;
        private IList<Flight>? _flights;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReaderService"/> class with the specified file path.
        /// </summary>
        /// <param name="filePath">The path to the CSV file containing flight data.</param>
        public FileReaderService(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Retrieves the list of flights from the CSV file.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains a list of <see cref="Flight"/> objects.
        /// </returns>
        public async Task<IList<Flight>> GetFlights()
        {
            _flights = _flights ?? await LoadData();
            return _flights;
        }

        /// <summary>
        /// Loads and parses flight data from the CSV file.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains a list of <see cref="Flight"/> objects.
        /// </returns>
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
