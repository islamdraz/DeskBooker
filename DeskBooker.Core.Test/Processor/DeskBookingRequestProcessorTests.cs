using DeskBooker.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using DeskBooker.Core.DataInterfaces;
using Moq;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private DeskBookingRequestProcessor _processor;
        private DeskBookingRequest _request;
        private Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private Mock<IDeskRepository> _deskRepositoryMock;
        private List<Desk> _availableDesks;

        public DeskBookingRequestProcessorTests()
        {
            _request = new DeskBookingRequest
            {
                FirstName = "islam",
                LastName = "draz",
                Email = "islamdraz@gmail.com",
                Date = new DateTime(2020, 1, 30)
            };
            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            _deskRepositoryMock=new Mock<IDeskRepository>();
            _availableDesks = new List<Desk>
            {
                new Desk{Id = 10}
            };
            _deskRepositoryMock.Setup(x => x.GetAvailableDesks(_request.Date)).Returns(
                _availableDesks
            );
             _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object,_deskRepositoryMock.Object);
           
        }
        [Fact]
        public void ShouldReturnDeskBookingResultWithRequestValues()
        {
            //Arrange
            
           
            //Act
            DeskBookingResult result = _processor.BookDesk(_request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.FirstName, _request.FirstName);
            Assert.Equal(result.LastName, _request.LastName);
            Assert.Equal(result.Email, _request.Email);
            Assert.Equal(result.Date, _request.Date);
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenPassNullToBookDesk()
        {
          
            var exception = Assert.Throws<ArgumentNullException>(()=> _processor.BookDesk(null));

            Assert.Equal(nameof(DeskBookingRequest),exception.ParamName);

        }


        [Fact]
        public void ShouldSaveDeskBookingRequest()
        {
            DeskBooking deskBookingShouldBeSaved = null;
            _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
                .Callback<DeskBooking>(deskbooking =>
            {
                deskBookingShouldBeSaved = deskbooking;
            });
            _processor.BookDesk(_request);


            _deskBookingRepositoryMock.Verify(x=>x.Save(It.IsAny<DeskBooking>()),Times.Once);

            Assert.NotNull(deskBookingShouldBeSaved);
            Assert.Equal(deskBookingShouldBeSaved.FirstName,_request.FirstName);
            Assert.Equal(deskBookingShouldBeSaved.LastName, _request.LastName);
            Assert.Equal(deskBookingShouldBeSaved.Email,_request.Email);
            Assert.Equal(deskBookingShouldBeSaved.Date,_request.Date);
            Assert.Equal(_availableDesks.First().Id,deskBookingShouldBeSaved.DeskId);
        }

        

        [Fact]
        public void BookDesk_DontCallSave_whenNoDeskAvailable()
        {
            //arrang
            
            _availableDesks.Clear();
            //act 
            _processor.BookDesk(_request);
            //assert

            _deskBookingRepositoryMock.Verify(x=>x.Save(It.IsAny<DeskBooking>()),Times.Never);
        }

     

        [Theory]
        [InlineData(DeskBookingResultCode.Success,true)]
        [InlineData(DeskBookingResultCode.NoDeskAvailabel,false)]
        public void BookDesk_shouldReturnResultCode_WhenDeskAvailableOrNOt(DeskBookingResultCode expectedResultCode,
            bool isDeskAvailable)
        {
            if (!isDeskAvailable)
            {
                _availableDesks.Clear();
            }

            var result=_processor.BookDesk(_request);

            Assert.Equal(expectedResultCode,result.Code);
        }

        [Theory]
        [InlineData(5, true)]
        [InlineData(null, false)]
        public void BookDesk_shouldReturnDeskBookingIdINResult_WhenDeskAvailableOrNOt(int? expectedDeskBookingId,
            bool isDeskAvailable)
        {
            if (!isDeskAvailable)
            {
                _availableDesks.Clear();
            }
            else
            {
                _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
                    .Callback<DeskBooking>(deskBooking => deskBooking.Id = expectedDeskBookingId.Value);
            }

            var result = _processor.BookDesk(_request);

            Assert.Equal(expectedDeskBookingId, result.BookingDeskId);
        }

    }
}
