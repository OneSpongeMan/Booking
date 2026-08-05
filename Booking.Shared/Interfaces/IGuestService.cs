using Booking.Shared.Models;

namespace Booking.Shared.Interfaces
{
    public interface IGuestService
    {
        Task<List<Guest>> GetAllAsync();
        Task<Guest?> GetByIdAsync(Guid id);
        Task<Guest?> GetByPhoneAsync(string phone);
        Task<List<Guest>> GetByNameAsync(string fullName);
        Task<List<Guest>> GetRegularCustomersAsync();
        Task AddAsync(Guest guest);
        Task UpdateAsync(Guest guest);
        Task DeleteAsync(Guid id);
    }
}
