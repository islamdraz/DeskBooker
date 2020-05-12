using System;
using System.Collections.Generic;
using DeskBooker.Core.Processor;

namespace DeskBooker.Core.Domain
{
    public class DeskBookingResult:DeskBookingBase
    {
        public int? BookingDeskId { get; set; }
        public DeskBookingResultCode Code { get; set; }
    }
}