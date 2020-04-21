using System;
using Cinema.Models.Tickets;

namespace Cinema.Models.Domain
{
    public class TimeSlot : BaseEntity
    {
        public DateTime StartTime { get; set; }
        public decimal Cost { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int HallId { get; set; }
        public Hall Hall { get; set; }

        public Format Format { get; set; }
        public TimeslotSeatRequest[] RequestedSeats { get; set; }
    }
}