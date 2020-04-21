using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cinema.Models.Domain;

namespace Cinema.Models.Tickets
{
    public class MovieListItem
    {
        public Movie Movie { get; set; }
        public TimeslotTag[] AvailableTimeslots { get; set; }
    }
}