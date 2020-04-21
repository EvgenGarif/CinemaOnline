using System.Linq;
using System.Web.Mvc;
using Cinema.Attributes;
using Cinema.Interfaces;
using Cinema.Models;
using Cinema.Models.Domain;
using LightInject;
using Newtonsoft.Json;

namespace Cinema.Controllers
{
    public class AdminController : Controller
    {
        [Inject]
        public ITicketsService TicketsService { get; set; }

        public ActionResult FindMovieById(int id)
        {
            var movie = TicketsService.GetMovieById(id);
            if (movie == null)
                return Content("Movie with such ID does not exist", "application/json");

            var movieJson = JsonConvert.SerializeObject(movie);
            return Content(movieJson, "application/json");
        }

        public ActionResult FindTimeslotById(int id)
        {
            var timeslot = TicketsService.GetTimeslotById(id);
            if (timeslot == null)
                return Content("TimeSlot with such ID does not exist", "application/json");

            var timeslotJson = JsonConvert.SerializeObject(timeslot);
            return Content(timeslotJson, "application/json");
        }

        public ActionResult FindHallById(int id)
        {
            var hall = TicketsService.GetHallById(id);
            if (hall == null)
                return Content("Hall with such ID does not exist", "application/json");

            var hallJson = JsonConvert.SerializeObject(hall);
            return Content(hallJson, "application/json");
        }

        public ActionResult MoviesList()
        {
            var movies = TicketsService.GetAllMovies();
            return View("MoviesList", movies);
        }

        [HttpGet]
        public ActionResult EditMovie(int movieId)
        {
            var movie = TicketsService.GetMovieById(movieId);
            return View("EditMovie", movie);
        }

        [HttpPost]
        public ActionResult EditMovie(Movie model)
        {
            if (ModelState.IsValid)
            {
                var updateResult = TicketsService.UpdateMovie(model);
                if (updateResult)
                {
                    return RedirectToAction("MoviesList");
                }

                return Content("Update failed.");
            }

            return View("EditMovie", model);
        }

        [HttpGet]
        public ActionResult HallsList()
        {
            var halls = TicketsService.GetAllHalls();
            return View("HallsList", halls);
        }

        [HttpGet]
        public ActionResult EditHall(int hallId)
        {
            var hall = TicketsService.GetHallById(hallId);
            return View("EditHall", hall);
        }

        [HttpPost]
        public ActionResult EditHall(Hall model)
        {
            if (ModelState.IsValid)
            {
                var updateResult = TicketsService.UpdateHall(model);
                if (updateResult)
                {
                    return RedirectToAction("HallsList");
                }

                return Content("Update failed.");
            }

            return View("EditHall", model);
        }

        [HttpGet]
        public ActionResult TimeslotsList(int? movieId = null)
        {
            ViewData["MovieId"] = movieId;
            return View("TimeslotsList", ProccessTimeslots(movieId == null ? TicketsService.GetAllTimeslots() : TicketsService.GetTimeslotsByMovieId(movieId.Value)));
        }

        private TimeslotGridRow[] ProccessTimeslots(TimeSlot[] timeslots)
        {
            if (!timeslots.Any())
                return new TimeslotGridRow[0];

            var movies = TicketsService.GetAllMovies();
            var halls = TicketsService.GetAllHalls();

            return timeslots.Select(timeslot => new TimeslotGridRow()
            {
                StartTime = timeslot.StartTime,
                Cost = timeslot.Cost,
                Format = timeslot.Format,
                Id = timeslot.Id,
                Hall = timeslot.Hall,
                Movie = timeslot.Movie
            }).ToArray();
        }

        [HttpGet]
        [PopulateHallsList, PopulateMoviesList]
        public ActionResult EditTimeslot(int timeslotId)
        {
            var timeslot = TicketsService.GetTimeslotById(timeslotId);
            return View("EditTimeslot", timeslot);
        }

        [HttpPost]
        [PopulateHallsList, PopulateMoviesList]
        public ActionResult EditTimeslot(TimeSlot model)
        {
            if (ModelState.IsValid)
            {
                var updateResult = TicketsService.UpdateTimeslot(model);
                if (updateResult)
                {
                    return RedirectToAction("TimeslotsList", new { movieId = model.MovieId });
                }

                return Content("Update failed.");
            }

            return View("EditTimeslot", model);
        }

        [HttpGet]
        public ActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMovie(Movie newMovie)
        {
            if (ModelState.IsValid)
            {
                var result = TicketsService.CreateMovie(newMovie);
                if (result)
                {
                    return RedirectToAction("MoviesList");
                }

                return Content("Update failed.");
            }

            return View(newMovie);
        }

        [HttpGet]
        [PopulateHallsList, PopulateMoviesList]
        public ActionResult AddTimeSlot()
        {
            return View();
        }

        [HttpPost]
        [PopulateHallsList, PopulateMoviesList]
        public ActionResult AddTimeSlot(TimeSlot newTimeSlot)
        {
            if (ModelState.IsValid)
            {
                var result = TicketsService.CreateTimeSlot(newTimeSlot);
                if (result)
                {
                    return RedirectToAction("TimeslotsList");
                }

                return Content("Update failed.");
            }

            return View(newTimeSlot);
        }

        [HttpGet]
        public ActionResult AddHall()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddHall(Hall newHall)
        {
            if (ModelState.IsValid)
            {
                var creationResult = TicketsService.CreateHall(newHall);
                if (creationResult)
                    return RedirectToAction("HallsList");


                return Content("Update failed. Please, contact system administrator.");
            }
            return View(newHall);
        }

        [HttpGet]
        public ActionResult RemoveMovie(int movieId)
        {
            var removeResult = TicketsService.RemoveMovie(movieId);
            if (removeResult)
                return RedirectToAction("MoviesList");

            return Content("Removing failed. Please, contact system administrator.");
        }

        public ActionResult RemoveHall(int hallId)
        {
            var removeResult = TicketsService.RemoveHall(hallId);
            if (removeResult)
                return RedirectToAction("HallsList");

            return Content("Removing failed. Please, contact system administrator.");
        }

        public ActionResult RemoveTimeslot(int timeslotId, int? movieId = null)
        {
            var removeResult = TicketsService.RemoveTimeslot(timeslotId);
            if (removeResult)
                return RedirectToAction("TimeslotsList", new { movieId });

            return Content("Removing failed. Please, contact system administrator.");
        }
    }
}