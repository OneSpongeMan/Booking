using System.ComponentModel.DataAnnotations;

namespace Booking.Shared.Models
{
    public class Table
    {
        [Key]
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int Seats { get; set; }
        public bool NearFountain { get; set; }


        public Table() { }

        public Table(Guid id, int number, int seats, bool nearFountain)
        {
            Id = id;
            Number = number;
            Seats = seats;
            NearFountain = nearFountain;
        }
    }
}
