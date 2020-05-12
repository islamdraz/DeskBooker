using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Domain;
using System;
using System.Linq;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessor : IDeskBookingRequestProcessor
    {
        public IDeskBookingRepository _deskBookingRepository;
        private readonly IDeskRepository _deskRepository;

        public DeskBookingRequestProcessor(IDeskBookingRepository deskBookingRepository,IDeskRepository deskRepository)
        {
            _deskBookingRepository = deskBookingRepository;
            _deskRepository = deskRepository;
        }

        public DeskBookingResult BookDesk(DeskBookingRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(DeskBookingRequest));
            var result = Create<DeskBookingResult>(request);
            
            if (_deskRepository.GetAvailableDesks(request.Date).FirstOrDefault() is Desk available)
            {
                var deskbooking = Create<DeskBooking>(request);
                deskbooking.DeskId = available.Id;
                _deskBookingRepository.Save(deskbooking);
                result.BookingDeskId = deskbooking.Id;
                result.Code = DeskBookingResultCode.Success;
            }
            else
            {
                result.Code = DeskBookingResultCode.NoDeskAvailabel;

            }
            return result;
        }

        public T Create<T>(DeskBookingRequest request)where T: DeskBookingBase,new()
        {
            return new T
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Date = request.Date
            };
        } 
    }
}