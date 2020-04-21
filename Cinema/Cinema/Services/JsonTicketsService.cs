using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cinema.Extensions;
using Cinema.Interfaces;
using Cinema.Models;
using Cinema.Models.Domain;
using Cinema.Models.Tickets;
using LightInject;
using Newtonsoft.Json;

namespace Cinema.Services
{
    public class JsonTicketsService : ITicketsService
    {
        private const string PathToJson = "/Files/Data.json";
        
        [Inject]
        public HttpContextBase Context { get; set; }

        public Movie GetMovieById(int id)
        {
            var fullModel = GetDataFromFile();
            return fullModel.Movies.FirstOrDefault(x => x.Id == id);
        }

        public Movie[] GetAllMovies()
        {
            var fullModel = GetDataFromFile();
            return fullModel.Movies;
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

        public Hall GetHallById(int id)
        {
            var fullModel = GetDataFromFile();
            return fullModel.Halls.FirstOrDefault(x => x.Id == id);
        }

        public Hall[] GetAllHalls()
        {
            var fullModel = GetDataFromFile();
            return fullModel.Halls;
        }

        public bool UpdateHall(Hall updatedHall)
        {
            var fullModel = GetDataFromFile();
            var hallToUpdate = fullModel.Halls.FirstOrDefault(x => x.Id == updatedHall.Id);
            if (hallToUpdate == null)
            {
                return false;
            }

            hallToUpdate.Name = updatedHall.Name;
            hallToUpdate.Places = updatedHall.Places;


            SaveToFile(fullModel);
            return true;
        }

        public TimeSlot GetTimeslotById(int id)
        {
            var fullModel = GetDataFromFile();
            return fullModel.TimeSlots.FirstOrDefault(x => x.Id == id);
        }

        public TimeSlot[] GetAllTimeslots()
        {
            var fullModel = GetDataFromFile();
            return fullModel.TimeSlots;
        }

        public bool UpdateTimeslot(TimeSlot updatedTimeslot)
        {
            var fullModel = GetDataFromFile();
            var timeSlotsToUpdate = fullModel.TimeSlots.FirstOrDefault(x => x.Id == updatedTimeslot.Id);
            if (timeSlotsToUpdate == null)
            {
                return false;
            }

            timeSlotsToUpdate.StartTime = updatedTimeslot.StartTime;
            timeSlotsToUpdate.Format = updatedTimeslot.Format;
            timeSlotsToUpdate.Cost = updatedTimeslot.Cost;
            timeSlotsToUpdate.MovieId = updatedTimeslot.MovieId;
            timeSlotsToUpdate.HallId = updatedTimeslot.HallId;

            SaveToFile(fullModel);
            return true;
        }

        public TimeSlot[] GetTimeslotsByMovieId(int movieId)
        {
            var fullModel = GetDataFromFile();
            return fullModel.TimeSlots.Where(x => x.MovieId == movieId).ToArray();
        }

        public TimeslotTag[] GetTimeslotTagsByMovieId(int movieId)
        {
            var timeslots = GetTimeslotsByMovieId(movieId);
            var resultModel = new List<TimeslotTag>();
            foreach (var timeslot in timeslots)
            {
                resultModel.Add(new TimeslotTag
                {
                    TimeslotId = timeslot.Id,
                    StartTime = timeslot.StartTime,
                    Cost = timeslot.Cost
                });
            }

            return resultModel.ToArray();
        }

        public bool UpdateMovie(Movie updatedMovie)
        {
            var fullModel = GetDataFromFile();
            var movieToUpdate = fullModel.Movies.FirstOrDefault(x => x.Id == updatedMovie.Id);
            if (movieToUpdate == null)
            {
                return false;
            }

            movieToUpdate.Title = updatedMovie.Title;
            movieToUpdate.MinAge = updatedMovie.MinAge;
            movieToUpdate.Director = updatedMovie.Director;
            movieToUpdate.Duration = updatedMovie.Duration;
            movieToUpdate.Description= updatedMovie.Description;
            movieToUpdate.ImgUrl = updatedMovie.ImgUrl;
            movieToUpdate.Rating = updatedMovie.Rating;
            movieToUpdate.ReleaseDate = updatedMovie.ReleaseDate;
            if (updatedMovie.Genres != null)
            {
                movieToUpdate.Genres = updatedMovie.Genres;
            }

            SaveToFile(fullModel);
            return true;
        }

        public bool CreateMovie(Movie newMovie)
        {
            var fullModel = GetDataFromFile();
            try
            {
                var newMovieId = fullModel.Movies.Max(m => m.Id) + 1;
                newMovie.Id = newMovieId;
                var existingMoviesList = fullModel.Movies.ToList();
                existingMoviesList.Add(newMovie);
                fullModel.Movies = existingMoviesList.ToArray();
                SaveToFile(fullModel);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public bool CreateTimeSlot(TimeSlot newTimeSlot)
        {
            var fullModel = GetDataFromFile();
            try
            {
                var newTimeSlotId = fullModel.TimeSlots.Max(m => m.Id) + 1;
                newTimeSlot.Id = newTimeSlotId;
                var existingTimeSlotList = fullModel.TimeSlots.ToList();
                existingTimeSlotList.Add(newTimeSlot);
                fullModel.TimeSlots = existingTimeSlotList.ToArray();
                SaveToFile(fullModel);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public bool AddRequestedSeatsToTimeSlot(SeatsProcessRequest request)
        {
            if (!(request?.SeatsRequest?.AddedSeats?.Any() ?? false)) return false;
            var fullModel = GetDataFromFile();
            var timeSlotsForUpdate = fullModel.TimeSlots.FirstOrDefault(x => x.Id == request.TimeslotId);
            if (timeSlotsForUpdate == null) return false;

            var requestToProcess = new List<TimeslotSeatRequest>();
            if (timeSlotsForUpdate.RequestedSeats != null && timeSlotsForUpdate.RequestedSeats.Any())
            {
                requestToProcess = timeSlotsForUpdate.RequestedSeats.ToList();
            }

            foreach (var addedSeat in request.SeatsRequest.AddedSeats)
            {
                requestToProcess.Add(new TimeslotSeatRequest()
                {
                    Row = addedSeat.Row,
                    Seat = addedSeat.Seat,
                    Status = request.SelectedStatus
                });
            }

            timeSlotsForUpdate.RequestedSeats = requestToProcess.ToArray();
            SaveToFile(fullModel);
            return true;
        }

        private void SaveToFile(FileModel model)
        {
            var jsonFilePath = Context.Server.MapPath(PathToJson);
            var serializedModel = JsonConvert.SerializeObject(model);
            System.IO.File.WriteAllText(jsonFilePath,serializedModel);
        }

        private FileModel GetDataFromFile()
        {
            var jsonFilePath = Context.Server.MapPath(PathToJson);
            if (!System.IO.File.Exists(jsonFilePath))
                return null;

            var json = System.IO.File.ReadAllText(jsonFilePath);
            var fileModel = JsonConvert.DeserializeObject<FileModel>(json);
            return fileModel;
        }

        public bool CreateHall(Hall newHall)
        {
            var fullModel = GetDataFromFile();
            try
            {
                var newHallId = 1;
                if (fullModel.Halls != null && fullModel.Halls.Any())
                {
                    newHallId = fullModel.Halls.Max(hall => hall.Id) + 1;
                }
                newHall.Id = newHallId;
                var existingHallsList = fullModel.Halls?.ToList() ?? new List<Hall>();
                existingHallsList.Add(newHall);
                fullModel.Halls = existingHallsList.ToArray();
                SaveToFile(fullModel);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool RemoveMovie(int id)
        {
            var fullModel = GetDataFromFile();
            try
            {
                var existingMoviesList = fullModel.Movies.ToList();
                existingMoviesList.RemoveAll(x => x.Id == id);
                fullModel.Movies = existingMoviesList.ToArray();
                SaveToFile(fullModel);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        public bool RemoveHall(int id)
        {
            var fullModel = GetDataFromFile();
            try
            {
                var existingHallsList = fullModel.Halls.ToList();
                existingHallsList.RemoveAll(x => x.Id == id);
                fullModel.Halls = existingHallsList.ToArray();
                SaveToFile(fullModel);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool RemoveTimeslot(int id)
        {
            var fullModel = GetDataFromFile();
            try
            {
                var existingTimeslotsList = fullModel.TimeSlots.ToList();
                existingTimeslotsList.RemoveAll(x => x.Id == id);
                fullModel.TimeSlots = existingTimeslotsList.ToArray();
                SaveToFile(fullModel);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        public MovieListItem[] SearchMoviesByTerm(string term)
        {
            var allMovies = GetFullMoviesInfo();
            var filteredList =
                allMovies.Where(x => x.Movie.Description.CaseInsensitiveContains(term) || x.Movie.Title.CaseInsensitiveContains(term));
            return filteredList.ToArray();
        }
    }
}