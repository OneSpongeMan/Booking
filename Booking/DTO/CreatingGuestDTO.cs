namespace Booking.DTO
{
    public class CreatingGuestDTO
    {
        public string Name { get; set; }
        public string? Surname { get; set; } = String.Empty;
        public string? Patronymic { get; set; } = String.Empty;
        public string Phone { get; set; }
    }
}
