using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using Cinema.DataBaseLayer;
using Cinema.Interfaces;
using Cinema.Models.Domain;
using Cinema.Models.Tickets;

namespace Cinema.Services
{
    public class EntityTicketsService : ITicketsService
    {
        public Movie GetMovieById(int id)
        {
            using (var context = new CinemaContext())
            {
                return context.Movies.First(x => x.Id == id);
            }
        }

        public Movie[] GetAllMovies()
        {
            using (var context = new CinemaContext())
            {
                return context.Movies.ToArray();
            }
        }

        private MovieListItem[] GetMoviesInfo(string term = "")
        {
            var model = new List<MovieListItem>();
            using (var ctx = new CinemaContext())
            {
                IQueryable<Movie> movieQuery = ctx.Movies;
                if (!string.IsNullOrEmpty(term))
                {
                    movieQuery = ctx.Movies.Where(x => x.Title.ToLowerInvariant().Contains(term));
                }

                foreach (var movie in movieQuery.ToArray())
                {
                    var movieListItem = new MovieListItem
                    {
                        Movie = movie,
                        AvailableTimeslots = ctx.TimeSlots.Where(timeslot => timeslot.MovieId == movie.Id)
                            .Select(timeslot => new TimeslotTag
                            {
                                Cost = timeslot.Cost,
                                StartTime = timeslot.StartTime,
                                TimeslotId = timeslot.Id
                            })
                            .ToArray()
                    };
                    model.Add(movieListItem);
                }
            }

            return model.ToArray();
        }

        public MovieListItem[] GetFullMoviesInfo()
        {
            return GetMoviesInfo();
        }

        public bool RemoveMovie(int id)
        {
            using (var ctx = new CinemaContext())
            {
                var movie = ctx.Movies.First(x => x.Id == id);
                movie.IsDeleted = true;
                ctx.SaveChanges();
            }

            return true;
        }

        public Hall GetHallById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Hall[] GetAllHalls()
        {
            using (var ctx = new CinemaContext())
            {
                return ctx.Halls.ToArray();
            }
        }

        public bool UpdateHall(Hall updatedHall)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveHall(int id)
        {
            throw new System.NotImplementedException();
        }

        public TimeSlot GetTimeslotById(int id)
        {
            throw new System.NotImplementedException();
        }

        public TimeSlot[] GetAllTimeslots()
        {
            using (var ctx = new CinemaContext())
            {
                return ctx.TimeSlots
                    .Include(x => x.Movie).Where(x => x.Movie != null)
                    .Include(x => x.Hall).Where(x => x.Hall != null)
                    .ToArray();
            }
        }

        public bool UpdateTimeslot(TimeSlot updatedTimeslot)
        {
            throw new System.NotImplementedException();
        }

        public TimeSlot[] GetTimeslotsByMovieId(int movieId)
        {
            using (var ctx = new CinemaContext())
            {
                return ctx.TimeSlots.ToArray();
            }
        }

        public bool RemoveTimeslot(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateMovie(Movie updatedMovie)
        {
            using (var context = new CinemaContext())
            {
                context.Movies.AddOrUpdate(updatedMovie);
                context.SaveChanges();
            }

            return true;
        }

        public bool CreateMovie(Movie newMovie)
        {
            throw new System.NotImplementedException();
        }

        public bool CreateTimeSlot(TimeSlot newTimeSlot)
        {
            throw new System.NotImplementedException();
        }

        public bool AddRequestedSeatsToTimeSlot(SeatsProcessRequest request)
        {
            throw new System.NotImplementedException();
        }

        public bool CreateHall(Hall newHall)
        {
            throw new System.NotImplementedException();
        }

        public MovieListItem[] SearchMoviesByTerm(string term)
        {
            return GetMoviesInfo(term);
        }
    }
}