using Booking.Shared.Models;

namespace Booking.Shared.Interfaces
{
    public interface ITableRepository
    {
        Task<List<Table>> GetAllAsync();
        Task<Table?> GetByNumberAsync(int number);
        Task<List<Table>> GetBySeatsAsync(int seats);
        Task<List<Table>> GetNearFountainAsync();
        Task AddAsync(Table table);
        Task UpdateAsync(Table table);
        Task DeleteAsync(Guid id);
    }
}
