using Booking.Shared.Models;

namespace Booking.Shared.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(Guid id);
        Task<List<Reservation>> GetByTimeAsync(DateTime start, DateTime end);
        Task<List<Reservation>> GetByGuestAsync(Guid guestId);
        Task<List<Reservation>> GetByTableAsync(int tableNum);
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Guid id);
    }
}
