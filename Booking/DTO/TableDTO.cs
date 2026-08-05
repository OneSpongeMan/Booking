using System.ComponentModel.DataAnnotations;

namespace Booking.DTO
{
    public class TableDTO
    {
        [Key]
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int Seats { get; set; }
        public bool NearFountain { get; set; }
    }
}
