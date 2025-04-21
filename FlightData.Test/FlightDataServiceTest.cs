using FlightData.Entities;
using FlightData.Services;
using FlightData.Services.Contracts;
using Moq;

namespace FlightData.Tests.Services
{
    [TestFixture]
    public class FlightDataServiceTests
    {
        private Mock<IDataReaderService> _mockDataReaderService;
        private Mock<IFlightSequenceValidator> _mockFlightSequenceValidator;
        private FlightDataService _flightDataService;

        [SetUp]
        public void SetUp()
        {
            _mockDataReaderService = new Mock<IDataReaderService>();
            _mockFlightSequenceValidator = new Mock<IFlightSequenceValidator>();
            _flightDataService = new FlightDataService(_mockDataReaderService.Object, _mockFlightSequenceValidator.Object);
        }

        [Test]
        public async Task GetFlightDataAsync_ReturnsAllFlights()
        {
            // Arrange
            var mockFlights = new List<Flight>
            {
                new Flight { FlightNumber = "A100" },
                new Flight { FlightNumber = "B200" }
            };

            _mockDataReaderService
                .Setup(x => x.GetFlights())
                .ReturnsAsync(mockFlights);

            // Act
            var result = await _flightDataService.GetFlightDataAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].FlightNumber, Is.EqualTo("A100"));
            Assert.That(result[1].FlightNumber, Is.EqualTo("B200"));
            _mockDataReaderService.Verify(x => x.GetFlights(), Times.Once);
        }

        [Test]
        public async Task GetFlightInconsistenciesAsync_ReturnsInconsistentFlights()
        {
            // Arrange
            var allFlights = new List<Flight>
            {
                new Flight { FlightNumber = "A100" },
                new Flight { FlightNumber = "B200" }
            };

            var inconsistentFlights = new List<Flight>
            {
                new Flight { FlightNumber = "B200" }
            };

            _mockDataReaderService
                .Setup(x => x.GetFlights())
                .ReturnsAsync(allFlights);

            _mockFlightSequenceValidator
                .Setup(x => x.Validate(allFlights))
                .Returns(inconsistentFlights);

            // Act
            var result = await _flightDataService.GetFlightInconsistenciesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FlightNumber, Is.EqualTo("B200"));
            _mockDataReaderService.Verify(x => x.GetFlights(), Times.Once);
            _mockFlightSequenceValidator.Verify(x => x.Validate(allFlights), Times.Once);
        }

        [Test]
        public async Task GetFlightDataAsync_ReturnsEmptyList_WhenNoFlightsAvailable()
        {
            // Arrange
            _mockDataReaderService
                .Setup(x => x.GetFlights())
                .ReturnsAsync(new List<Flight>());

            // Act
            var result = await _flightDataService.GetFlightDataAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetFlightInconsistenciesAsync_ReturnsEmptyList_WhenNoInconsistencies()
        {
            // Arrange
            var allFlights = new List<Flight>
            {
                new Flight { FlightNumber = "A100" }
            };

            _mockDataReaderService
                .Setup(x => x.GetFlights())
                .ReturnsAsync(allFlights);

            _mockFlightSequenceValidator
                .Setup(x => x.Validate(allFlights))
                .Returns(new List<Flight>());

            // Act
            var result = await _flightDataService.GetFlightInconsistenciesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
