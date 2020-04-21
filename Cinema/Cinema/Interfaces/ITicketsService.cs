using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Models.Domain;
using Cinema.Models.Tickets;

namespace Cinema.Interfaces
{
    public interface ITicketsService
    {
        Movie GetMovieById(int id);
        Movie[] GetAllMovies();
        MovieListItem[] GetFullMoviesInfo();
        bool RemoveMovie(int id);

        Hall GetHallById(int id);
        Hall[] GetAllHalls();
        bool UpdateHall(Hall updatedHall);
        bool RemoveHall(int id);

        TimeSlot GetTimeslotById(int id);
        TimeSlot[] GetAllTimeslots();
        bool UpdateTimeslot(TimeSlot updatedTimeslot);
        TimeSlot[] GetTimeslotsByMovieId(int movieId);
        bool RemoveTimeslot(int id);

        bool UpdateMovie(Movie updatedMovie);
        bool CreateMovie(Movie newMovie);
        bool CreateTimeSlot(TimeSlot newTimeSlot);
        bool AddRequestedSeatsToTimeSlot(SeatsProcessRequest request);
        bool CreateHall(Hall newHall);
        MovieListItem[] SearchMoviesByTerm(string term);
    }
}