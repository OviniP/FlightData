using FlightData.Entities;
using FlightData.Services;

namespace FlightData.Tests
{
    [TestFixture]
    public class FlightSequenceValidatorTests
    {
        private FlightSequenceValidator _flightSequenceValidator;

        [SetUp]
        public void SetUp()
        {
            _flightSequenceValidator = new FlightSequenceValidator();
        }

        [Test]
        public void Validate_ShouldReturnInconsistentFlights_WhenArrivalIsBeforeDeparture()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-20 10:00:00",
                    ArrivalDateTime = "2025-04-20 09:00:00",
                    DepartureAirport = "JFK",
                    ArrivalAirport = "LAX"
                }
            };

            // Act
            var result = _flightSequenceValidator.Validate(flights);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(flights[0]));
        }

        [Test]
        public void Validate_ShouldReturnInconsistentFlights_WhenDepartureIsBeforePreviousArrival()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-20 10:00:00",
                    ArrivalDateTime = "2025-04-20 12:00:00",
                    DepartureAirport = "JFK",
                    ArrivalAirport = "LAX"
                },
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-20 11:30:00",
                    ArrivalDateTime = "2025-04-20 12:30:00",
                    DepartureAirport = "LAX",
                    ArrivalAirport = "SFO"
                }
            };

            // Act
            var result = _flightSequenceValidator.Validate(flights);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(flights[1]));
        }

        [Test]
        public void Validate_ShouldReturnInconsistentFlights_WhenLocationIsInconsistent()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-20 10:00:00",
                    ArrivalDateTime = "2025-04-20 12:00:00",
                    DepartureAirport = "JFK",
                    ArrivalAirport = "LAX"
                },
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-20 13:00:00",
                    ArrivalDateTime = "2025-04-20 15:00:00",
                    DepartureAirport = "HEL",
                    ArrivalAirport = "JFK"
                }
            };

            // Act
            var result = _flightSequenceValidator.Validate(flights);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(flights[1]));
        }

        [Test]
        public void Validate_ShouldReturnEmptyList_WhenNoInconsistencies()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-20 10:00:00",
                    ArrivalDateTime = "2025-04-20 12:00:00",
                    DepartureAirport = "JFK",
                    ArrivalAirport = "LAX"
                },
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-20 13:00:00",
                    ArrivalDateTime = "2025-04-20 15:00:00",
                    DepartureAirport = "LAX",
                    ArrivalAirport = "SFO"
                }
            };

            // Act
            var result = _flightSequenceValidator.Validate(flights);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Validate_ShouldHandleMultipleFlightNumbersCorrectly()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-20 10:00:00",
                    ArrivalDateTime = "2025-04-20 12:00:00",
                    DepartureAirport = "JFK",
                    ArrivalAirport = "LAX"
                },
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-20 13:00:00",
                    ArrivalDateTime = "2025-04-20 15:00:00",
                    DepartureAirport = "LAX",
                    ArrivalAirport = "SFO"
                },
                new Flight
                {
                    FlightNumber = "B456",
                    DepartureDateTime = "2025-04-20 10:00:00",
                    ArrivalDateTime = "2025-04-20 11:30:00",
                    DepartureAirport = "SFO",
                    ArrivalAirport = "ORD"
                },
                new Flight
                {
                    FlightNumber = "B456",
                    DepartureDateTime = "2025-04-20 12:30:00",
                    ArrivalDateTime = "2025-04-20 14:00:00",
                    DepartureAirport = "ORD",
                    ArrivalAirport = "JFK"
                }
            };

            // Act
            var result = _flightSequenceValidator.Validate(flights);

            // Assert
            Assert.That(result, Is.Empty); // No inconsistencies for either flight number
        }

        [Test]
        public void Validate_ShouldReturnOnce_WhenMultipleInconsistenciesInSameFlight()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-20 10:00:00",
                    ArrivalDateTime = "2025-04-20 09:00:00", // Arrival before departure
                    DepartureAirport = "JFK",
                    ArrivalAirport = "LAX"
                },
                new Flight
                {
                    FlightNumber = "A123",
                    DepartureDateTime = "2025-04-21 13:00:00",
                    ArrivalDateTime = "2025-04-21 15:00:00",
                    DepartureAirport = "LAX",
                    ArrivalAirport = "HEL"
                }
            };

            // Act
            var result = _flightSequenceValidator.Validate(flights);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1)); // Only one inconsistent flight should be returned
            Assert.That(result[0], Is.EqualTo(flights[0])); // The inconsistent flight should be the one with issues
        }



    }
}
