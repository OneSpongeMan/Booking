using Booking.Shared.Models;

namespace Booking.Shared.Interfaces
{
    public interface IGuestRepository
    {
        Task<List<Guest>> GetAllAsync();
        Task<Guest?> GetByIdAsync(Guid id);
        Task<Guest?> GetByPhoneAsync(string phone);
        Task<List<Guest>> GetByNameAsync(string surname, string name, string patronymic);
        Task<List<Guest>> GetRegularCustomersAsync();
        Task AddAsync(Guest guest);
        Task UpdateAsync(Guest guest);
        Task DeleteAsync(Guid id);
    }
}
