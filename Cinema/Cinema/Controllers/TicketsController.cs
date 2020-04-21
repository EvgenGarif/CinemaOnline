using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cinema.Interfaces;
using Cinema.Models.Domain;
using Cinema.Models.Tickets;
using Cinema.Services;
using Newtonsoft.Json;

namespace Cinema.Controllers
{
    public class TicketsController : Controller
    {

        private readonly ITicketsService _ticketsService;

        public TicketsController(ITicketsService ticketsService)
        {
            _ticketsService = ticketsService;
        }

        public ActionResult GetMovies()
        {
            var allMovies = _ticketsService.GetFullMoviesInfo();
            return View("~/Views/Tickets/MoviesList.cshtml", allMovies);
        }

        public ActionResult GetHallInfo(int timeslotId)
        {
            var timeSlot = _ticketsService.GetTimeslotById(timeslotId);
            var model = new HallInfo()
            {
                ColumnsCount = 20,
                RowsCount = 12,
                TicketCost = timeSlot.Cost,
                CurrentTimeSlotId = timeslotId,
                RequestedSeats = timeSlot.RequestedSeats
            };
            return View("HallInfo", model);
        }

        public string ProcessRequest(SeatsProcessRequest request)
        {
            var result = _ticketsService.AddRequestedSeatsToTimeSlot(request);
            return JsonConvert.SerializeObject(new {requestResult = result});
        }

        [HttpPost]
        public string SearchFilms(string term)
        {
            var allResults = _ticketsService.SearchMoviesByTerm(term);
            return JsonConvert.SerializeObject(new { Result = allResults });
        }
    
    }
}