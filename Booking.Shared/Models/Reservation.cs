using System.ComponentModel.DataAnnotations;

namespace Booking.Shared.Models
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }
        public int Table { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public Guid Guest {  get; set; }
        public int PersonsNumber { get; set; }
        public bool TempBooked { get; set; }
        public string? Comment { get; set; }


        public Reservation() { }

        public Reservation(Guid id, int table, DateTime start, DateTime end, Guid guest, int persons, bool tempBooked, string comment)
        {
            Id = id;
            Table = table;
            Start = start;
            End = end;
            Guest = guest;
            PersonsNumber = persons;
            TempBooked = tempBooked;
            Comment = comment;
        }
    }
}
