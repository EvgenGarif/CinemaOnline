using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Models.Reports
{
    public class PotentialRealProfitReportModel
    {
        public IEnumerable<PotentialRealProfitReportRow> Rows { get; set; }
    }
}