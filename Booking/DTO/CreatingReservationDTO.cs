namespace Booking.DTO
{
    public class CreatingReservationDTO
    {
        public int Table { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Guid Guest { get; set; }
        public int PersonsNumber { get; set; }
        public bool TempBooked { get; set; }
        public string? Comment { get; set; }
    }
}
