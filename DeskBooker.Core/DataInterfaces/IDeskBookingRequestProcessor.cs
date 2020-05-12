using DeskBooker.Core.Domain;

namespace DeskBooker.Core.DataInterfaces
{
    public interface IDeskBookingRequestProcessor
    {
        DeskBookingResult BookDesk(DeskBookingRequest request);
    }
}