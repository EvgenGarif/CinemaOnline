using System;
using Cinema.Models.Domain;

namespace Cinema.Models
{
    public class TimeslotGridRow
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Cost { get; set; }
        public Movie Movie { get; set; }
        public Hall Hall { get; set; }
        public Format Format { get; set; }
    }
}