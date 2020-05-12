using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeskBooker.Core.DataInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using DeskBooker.Persistance;

namespace DeskBooker.Web
{
    public class BookDeskModel : PageModel
    {
        private readonly IDeskBookingRequestProcessor _deskBookingRequestProcessor;

        public BookDeskModel(IDeskBookingRequestProcessor deskBookingRequestProcessor)
        {
            _deskBookingRequestProcessor = deskBookingRequestProcessor;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DeskBookingRequest DeskBookingRequest { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

          var result=  _deskBookingRequestProcessor.BookDesk(DeskBookingRequest);
          if (result.Code == DeskBookingResultCode.NoDeskAvailabel)
          {
              ModelState.AddModelError("NoDeskAvailableINThatDate", "no Desk Available At this date");
              return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
