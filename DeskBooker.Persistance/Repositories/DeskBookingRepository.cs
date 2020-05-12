using System;
using System.Collections.Generic;
using System.Text;
using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Domain;

namespace DeskBooker.Persistance.Repositories
{
public    class DeskBookingRepository:IDeskBookingRepository
    {
        private readonly DeskBookerContext _context;

        public DeskBookingRepository(DeskBookerContext context)
        {
            _context = context;
        }
        public void Save(DeskBooking deskBooking)
        {
            _context.DeskBookings.Add(deskBooking);
            _context.SaveChanges();
        }
    }
}
