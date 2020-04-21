using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cinema.Models.Tickets
{
    public class TimeslotTag
    {
        public int TimeslotId { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Cost { get; set; }

        public string FormattedStartTime => StartTime.ToShortTimeString();

        public string BuyTicketUrl => UrlHelper.GenerateUrl(null, "HallInfo", "Tickets", new RouteValueDictionary(new { timeslotId = TimeslotId }), RouteTable.Routes, HttpContext.Current.Request.RequestContext, false);
    }
}