using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cinema.Extensions;
using Newtonsoft.Json;

namespace Cinema.Models.Domain
{
    public class Movie : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Duration { get; set; }
        public Genre[] Genres { get; set; }
        public int MinAge { get; set; }
        public string Director { get; set; }
        public string ImgUrl { get; set; }
        public double Rating { get; set; }
        public int? ReleaseDate { get; set; }

        public ICollection<TimeSlot> Timeslots { get; set; }

        [JsonIgnore]
        public string FormattedGenres => string.Join(", ", Genres);

        [JsonIgnore]
        public string FormattedDuration => Duration.ToDuration();

    }
}