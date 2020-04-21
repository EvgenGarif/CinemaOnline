namespace Cinema.Models.Tickets
{
    public class SeatsRequest
    {
        public SelectedSeat[] AddedSeats { get; set; }
        public decimal Sum { get; set; }
    }
}