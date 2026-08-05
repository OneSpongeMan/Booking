using System.ComponentModel.DataAnnotations;

namespace Booking.DTO
{
    public class GuestDTO
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Surname { get; set; } = String.Empty;
        public string? Patronymic { get; set; } = String.Empty;
        public string Phone { get; set; }
        public bool RegularCustomer { get; set; } = false;
    }
}
