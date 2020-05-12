using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Domain;

namespace DeskBooker.Persistance.Repositories
{
public    class DeskRepository:IDeskRepository
    {
        private readonly DeskBookerContext _context;

        public DeskRepository(DeskBookerContext context)
        {
            _context = context;
        }
        public IEnumerable<Desk> GetAvailableDesks(DateTime date)
        {
            return _context.Desks.ToList();
        }
    }
}
