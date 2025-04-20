using FlightData.Services;

namespace FlightData.Test
{
    public class FileReaderServiceTests
    {
        private FileReaderService _fileReaderForValidFile;
        private FileReaderService _fileReaderForNoRecordFile;
        
        [SetUp]
        public void Setup()
        {
            _fileReaderForValidFile = GetReader("flights.csv");
            _fileReaderForNoRecordFile = GetReader("noRecords.csv");
        }

        [Test]
        public void LoadData_WhenDataExists_ShouldReturnFlights()
        {
            var result = _fileReaderForValidFile.GetFlights();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task LoadData_WhenDataExists_ShouldReturnAll()
        {
            var result = await _fileReaderForValidFile.GetFlights();
            int recordCount = 1000;
            Assert.That(result.Count, Is.EqualTo(recordCount));
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task LoadData_WhenNoData_ShouldReturnZero()
        {
            var result = await _fileReaderForNoRecordFile.GetFlights();
            int recordCount = 0;
            Assert.That(result.Count, Is.EqualTo(recordCount));
            Assert.That(result, Is.Not.Empty);
        }

        private FileReaderService GetReader(string fileName)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", fileName);
            return new FileReaderService(filePath);
        }
    }
}