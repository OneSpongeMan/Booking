using System.ComponentModel.DataAnnotations;

namespace Booking.DTO
{
    public class GuestDTO
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string Phone { get; set; }
    }
}
