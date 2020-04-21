using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AutoMapper;
using Cinema.Interfaces;
using Cinema.Models.Domain;
using Cinema.Models.Tickets;
using Cinema.Utils;
using System.Web.UI.WebControls;

namespace Cinema.Services
{
    public class SqlTicketsService : ITicketsService
    {
        private readonly SqlDatabaseUtil _sqlDatabaseUtil;

        public SqlTicketsService(IMapper mapper)
        {
            _sqlDatabaseUtil = new SqlDatabaseUtil(mapper);
        }

        public Movie GetMovieById(int id)
        {
            return _sqlDatabaseUtil.Select<Movie>("SELECT * FROM movies WHERE id = @id", new SqlParameter("@id", id)).FirstOrDefault();
        }

        public Movie[] GetAllMovies()
        {
            return _sqlDatabaseUtil.Select<Movie>("SELECT * FROM movies").ToArray();
        }

        public MovieListItem[] GetFullMoviesInfo()
        {
            var allMovies = GetAllMovies();
            var resultModel = new List<MovieListItem>();
            foreach (var movie in allMovies)
            {
                resultModel.Add(
                    new MovieListItem
                    {
                        Movie = movie,
                        AvailableTimeslots = GetTimeslotTagsByMovieId(movie.Id)
                    });
            }

            return resultModel.ToArray();
        }

        public bool RemoveMovie(int movieId)
        {
            return _sqlDatabaseUtil.Execute("DELETE FROM movies WHERE id=@id",
                new SqlParameter("@id", movieId));
        }

        public Hall GetHallById(int id)
        {
            return _sqlDatabaseUtil.Select<Hall>("SELECT * FROM movies WHERE id =@id", new SqlParameter("@id", id))
                .FirstOrDefault();
        }

        public Hall[] GetAllHalls()
        {
            return _sqlDatabaseUtil.Select<Hall>("SELECT * FROM halls").ToArray();
        }

        public bool UpdateHall(Hall updatedHall)
        {
            return _sqlDatabaseUtil.Execute("UPDATE hall SET Name = @Name, Places=@Places WHERE id = @id",
                new SqlParameter("@id", updatedHall.Id),
                new SqlParameter("@Name", updatedHall.Name),
                new SqlParameter("@Places", updatedHall.Places));
        }

        public bool RemoveHall(int hallId)
        {
            return _sqlDatabaseUtil.Execute("DELETE FROM halls WHERE id=@id",
                new SqlParameter("@id", hallId));
        }

        public TimeSlot GetTimeslotById(int id)
        {
            var timeSlot = _sqlDatabaseUtil.Select<TimeSlot>("SELECT * FROM timeslots WHERE id = @id", new SqlParameter("@id", id))
                .FirstOrDefault();

            if (timeSlot != null)
            {
                timeSlot.RequestedSeats = _sqlDatabaseUtil.Select<TimeslotSeatRequest>(
                    "SELECT * FROM requestedSeats WHERE timeslotid = @timeslotid ",
                    new SqlParameter("@timeslotid", id)).ToArray();
            }

            return timeSlot;
        }

        public TimeSlot[] GetAllTimeslots()
        {
            return _sqlDatabaseUtil.Select<TimeSlot>("SELECT * FROM timeslots").ToArray();
        }

        public bool UpdateTimeslot(TimeSlot updatedTimeslot)
        {
            return _sqlDatabaseUtil.Execute("UPDATE timeslots SET starttime = @starttime, cost = @cost, movieid = @movieid, hallid = @hallid, format = @format  WHERE id=@id",
                new SqlParameter("@id", updatedTimeslot.Id),
                new SqlParameter("@starttime", updatedTimeslot.StartTime),
                new SqlParameter("@cost", updatedTimeslot.Cost),
                new SqlParameter("@movieid", updatedTimeslot.MovieId),
                new SqlParameter("@hallid", updatedTimeslot.HallId),
                new SqlParameter("@format", updatedTimeslot.Format.ToString()));
        }

        public TimeSlot[] GetTimeslotsByMovieId(int movieId)
        {
            return _sqlDatabaseUtil.Select<TimeSlot>("SELECT * FROM timeslots WHERE movieid=@movieid",
                new SqlParameter("@movieid", movieId)).ToArray();
        }

        public TimeslotTag[] GetTimeslotTagsByMovieId(int movieId)
        {
            var timeslots = _sqlDatabaseUtil.Select<TimeSlot>("SELECT * FROM timeslots WHERE movieid=@movieid",
                new SqlParameter("@movieid", movieId)).ToArray();

            var resultModel = new List<TimeslotTag>();
            foreach (var timeslot in timeslots)
            {
                resultModel.Add(new TimeslotTag
                {
                    TimeslotId = timeslot.Id,
                    StartTime = timeslot.StartTime,
                    Cost = timeslot.Cost,
                });
            }

            return resultModel.ToArray();
        }

        public bool RemoveTimeslot(int timeslotId)
        {
            return _sqlDatabaseUtil.Execute("DELETE FROM timeslots WHERE id=@id",
                new SqlParameter("@id", timeslotId));
        }

        public bool UpdateMovie(Movie updatedMovie)
        {
            return _sqlDatabaseUtil.Execute("update movies set title = @title where id = @id",
                new SqlParameter("@id", updatedMovie.Id), new SqlParameter("@title", updatedMovie.Title));
        }

        public bool CreateMovie(Movie newMovie)
        {
            return _sqlDatabaseUtil.Execute(
                "INSERT INTO Movies([Title],[Description],[Duration],[MinAge],[Director],[ImgUrl],[Rating],[ReleaseDate],[Country],[Genres]) VALUES (@title, @description, @duration, @minage, @director, @imgurl, @rating, @releasedate, @country, @genres)",
                new SqlParameter("@title", newMovie.Title),
                new SqlParameter("@description", newMovie.Description),
                new SqlParameter("@duration", newMovie.Duration),
                new SqlParameter("@minage", newMovie.MinAge),
                new SqlParameter("@director", newMovie.Director),
                new SqlParameter("@imgurl", newMovie.ImgUrl),
                new SqlParameter("@rating", newMovie.Rating),
                new SqlParameter("@releasedate", newMovie.ReleaseDate),
                new SqlParameter("@genres", String.Join(",", newMovie.Genres)));
        }

        public bool CreateTimeSlot(TimeSlot newTimeSlot)
        {
            return _sqlDatabaseUtil.Execute(
                "INSERT INTO timeslots ([StartTime], [Cost], [MovieId], [HallId], [Format]) VALUES (@starttime, @cost, @movieid, @hallid, @format)",
                new SqlParameter("@starttime", newTimeSlot.StartTime),
                new SqlParameter("@cost", newTimeSlot.Cost),
                new SqlParameter("@movieid", newTimeSlot.MovieId),
                new SqlParameter("@format", newTimeSlot.Format),
                new SqlParameter("@hallid", newTimeSlot.HallId));
        }

        public bool AddRequestedSeatsToTimeSlot(SeatsProcessRequest request)
        {
            if (!(request?.SeatsRequest?.AddedSeats?.Any() ?? false)) return false;

            foreach (var seatsRequestAddedSeat in request.SeatsRequest.AddedSeats)
            {
                _sqlDatabaseUtil.Execute("INSERT INTO requestedseats ([timeslotid], [row], [seat], [status]) values (@timeslotid,@row, @seat, @status)",
                    new SqlParameter("@timeslotid", request.TimeslotId),
                    new SqlParameter("@row", seatsRequestAddedSeat.Row),
                    new SqlParameter("@seat", seatsRequestAddedSeat.Seat),
                    new SqlParameter("@status", request.SelectedStatus.ToString()));
            }

            return true;
        }

        public bool CreateHall(Hall newHall)
        {
            return _sqlDatabaseUtil.Execute(
                "INSERT INTO halls ([Name], [Places]) VALUES (@name, @places)",
                new SqlParameter("@name", newHall.Name),
                new SqlParameter("@places", newHall.Places));
        }

        public MovieListItem[] SearchMoviesByTerm(string term)
        {
            throw new NotImplementedException();
        }
    }
}