using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;

namespace DeskBooker.Web.Test.Pages
{
public    class BookDeskModelTest
    {
        private Mock<IDeskBookingRequestProcessor> deskBookingProcessorMock;
        private BookDeskModel bookDeskModel;
        private DeskBookingResult _deskBookingResult;

        public BookDeskModelTest()
        {
             deskBookingProcessorMock = new Mock<IDeskBookingRequestProcessor>(); 
             bookDeskModel = new BookDeskModel(deskBookingProcessorMock.Object);

             _deskBookingResult = new DeskBookingResult
             {
                 Code=DeskBookingResultCode.Success
             };
             deskBookingProcessorMock.Setup(x => x.BookDesk(bookDeskModel.DeskBookingRequest)).Returns(
                _deskBookingResult
            );
        }

        [Theory]
        [InlineData(0,false)]
        [InlineData(1,true)]
        public async Task ShouldCallDeskBookingWhenPostIfModelIsValide(int numberOfCalls,bool isModelValide)
        {
           
          
           
             if (!isModelValide)
             {
                bookDeskModel.ModelState.AddModelError("invalideError","this is not valide");
             }

            await bookDeskModel.OnPostAsync();

            deskBookingProcessorMock.Verify(x=>x.BookDesk(It.IsAny<DeskBookingRequest>()),Times.Exactly(numberOfCalls));
        }


        [Fact]
        public async Task ShouldReturnErrorIfNODeskAvailable()
        {
            _deskBookingResult.Code = DeskBookingResultCode.NoDeskAvailabel;
            await bookDeskModel.OnPostAsync();
            //Assert
            var modelErrors = Assert.Contains("NoDeskAvailableINThatDate", bookDeskModel.ModelState);
            var modelError = Assert.Single(modelErrors.Errors);

            Assert.Equal("no Desk Available At this date", modelError.ErrorMessage);
        }                  

        [Fact]
        public async Task ShouldNotReturnErrorIfDeskAvailable()
        {
            
            await bookDeskModel.OnPostAsync();
            //Assert
            Assert.DoesNotContain("NoDeskAvailableINThatDate", bookDeskModel.ModelState);

        }


        [Theory]
        [InlineData(DeskBookingResultCode.NoDeskAvailabel)]
        [InlineData(DeskBookingResultCode.Success)]
        public async Task ShouldReturnCorrectActionReslultIFDeskAvailableAndReslultCodeSuccess(DeskBookingResultCode resultCode)
        {
            IActionResult result = null;
            _deskBookingResult.Code = resultCode;

            result = await bookDeskModel.OnPostAsync();

            if (resultCode == DeskBookingResultCode.NoDeskAvailabel)
                Assert.IsType<PageResult>(result);

            if (resultCode == DeskBookingResultCode.Success)
                Assert.IsType<RedirectToPageResult>(result);

        }


    }
}
