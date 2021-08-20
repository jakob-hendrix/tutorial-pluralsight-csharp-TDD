using System;
using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;
using Moq;
using Xunit;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private readonly DeskBookingRequestProcessor _processor;
        private readonly DeskBookingRequest _request;
        private readonly Mock<IDeskBookingRepository> _deskBookingRepositoryMock;

        public DeskBookingRequestProcessorTests()
        {
            _request = new DeskBookingRequest
            {
                FirstName = "Jakob",
                LastName = "Hendrix",
                Email = "jhendrix001@gmail.com",
                Date = new DateTime(2021, 8, 18)
            };

            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object);
        }

        [Fact]
        public void ShouldReturnDeskBookingResultWithRequestValues()
        {
            // Act
            DeskBookingResult result = _processor.BookDesk(_request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_request.FirstName, result.FirstName);
            Assert.Equal(_request.LastName, result.LastName);
            Assert.Equal(_request.Email, result.Email);
            Assert.Equal(_request.Date, result.Date);
        }

        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveDeskBooking()
        {
            // Arrange
            DeskBooking savedDeskBooking = null;

            _deskBookingRepositoryMock
                .Setup(x => x.Save(It.IsAny<DeskBooking>()))
                .Callback<DeskBooking>(deskBooking =>
                {
                    savedDeskBooking = deskBooking;
                });

            // Act
            _processor.BookDesk(_request);

            // Assert
            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Once);
            Assert.NotNull(savedDeskBooking);
            Assert.Equal(_request.FirstName, savedDeskBooking.FirstName);
            Assert.Equal(_request.LastName, savedDeskBooking.LastName);
            Assert.Equal(_request.Email, savedDeskBooking.Email);
            Assert.Equal(_request.Date, savedDeskBooking.Date);
        }
    }
}

