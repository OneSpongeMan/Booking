using AutoMapper;
using Booking.DTO;
using Booking.Shared.Models;

namespace Booking
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreatingGuestDTO, Guest>();
            CreateMap<CreatingReservationDTO, Reservation>();
            CreateMap<CreatingTableDTO, Table>();

            CreateMap<Guest, GuestDTO>().ReverseMap();
            CreateMap<Reservation, ReservationDTO>().ReverseMap();
            CreateMap<Table, TableDTO>().ReverseMap();
        }
    }
}
