using FlightData.Entities;
using FlightData.Services;
using Microsoft.Extensions.Options;

namespace FlightData.Tests.Services
{
    [TestFixture]
    public class FileReaderServiceTests
    {
        private string _tempFilePath;
        private FileReaderService _fileReaderService;

        [SetUp]
        public void SetUp()
        {
            // Create a temp CSV file with sample data
            _tempFilePath = Path.GetTempFileName();

            var csvContent = GetSampleCsvContent();

            File.WriteAllText(_tempFilePath, csvContent);

            var options = Options.Create(new ApiSettings { DataFilePath = _tempFilePath });
            _fileReaderService = new FileReaderService(options);
        }

        //[OneTimeTearDown]
        public void TearDown()
        {
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        [Test]
        public async Task GetFlights_ReturnsFlightsFromCsv()
        {
            // Act
            var flights = await _fileReaderService.GetFlights();

            // Assert
            Assert.That(flights, Is.Not.Null);
            Assert.That(flights.Count, Is.EqualTo(2));
            Assert.That(flights[0].FlightNumber, Is.EqualTo("M645"));
            Assert.That(flights[1].DepartureAirport, Is.EqualTo("SFO"));
        }

        [Test]
        public async Task GetFlights_ReturnsEmptyList_WhenNoFlightsInCsv()
        {
            // Arrange
            File.WriteAllText(_tempFilePath, GetCsvHeaders()); // only headers, no data
            var options = Options.Create(new ApiSettings { DataFilePath = _tempFilePath });
            var service = new FileReaderService(options);

            // Act
            var result = await service.GetFlights();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }


        [Test]
        public async Task GetFlights_ReturnsCachedFlights_OnSecondCall()
        {
            // Act
            var firstCall = await _fileReaderService.GetFlights();
            var secondCall = await _fileReaderService.GetFlights();

            // Assert
            Assert.That(firstCall, Is.SameAs(secondCall), "Should return the same cached list instance");
        }

        [Test]
        public void GetFlights_Throws_WhenFileIsMissing()
        {
            // Arrange
            File.Delete(_tempFilePath);
            var options = Options.Create(new ApiSettings { DataFilePath = _tempFilePath });
            var readerService = new FileReaderService(options);

            // Act & Assert
            Assert.ThrowsAsync<FileNotFoundException>(() => readerService.GetFlights());
        }

        #region Private Methods
        private string GetSampleCsvContent()
        {
            return
                GetCsvHeaders() +
                "1,ZX-IKD,350,M645,HEL,2024-01-02 21:46:27,DXB,2024-01-03 02:31:27\n" +
                "2,ZW-TNZ,787,K319,SFO,2023-03-25 15:55:27,DXB,2023-03-25 17:53:27";
        }

        private string GetCsvHeaders() {
            return "id,aircraft_registration_number,aircraft_type,flight_number,departure_airport,departure_datetime,arrival_airport,arrival_datetime\n";
        }

        #endregion

    }
}
