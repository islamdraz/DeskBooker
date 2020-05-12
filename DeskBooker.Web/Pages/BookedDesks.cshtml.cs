using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DeskBooker.Core.Domain;
using DeskBooker.Persistance;

namespace DeskBooker.Web
{
    public class BookedDesksModel : PageModel
    {
        private readonly DeskBooker.Persistance.DeskBookerContext _context;

        public BookedDesksModel(DeskBooker.Persistance.DeskBookerContext context)
        {
            _context = context;
        }

        public IList<DeskBooking> DeskBooking { get;set; }

        public async Task OnGetAsync()
        {
            DeskBooking = await _context.DeskBookings.ToListAsync();
        }
    }
}
