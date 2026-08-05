using System.ComponentModel.DataAnnotations;

namespace Booking.Shared.Models
{
    public class Guest
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string Phone {  get; set; }
        public bool RegularCustomer { get; set; } = false;


        public Guest() { }

        public Guest(Guid id, string name, string? surname, string? patronymic, string phone, bool regularCustomer)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Phone = phone;
            RegularCustomer = regularCustomer;
        }
    }
}
