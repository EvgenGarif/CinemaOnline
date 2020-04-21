namespace Cinema.Models.Tickets
{
    public class HallInfo
    {
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }
        public decimal TicketCost { get; set; }
        public int CurrentTimeSlotId { get; set; }
        public TimeslotSeatRequest[] RequestedSeats { get; set; }

    }
}