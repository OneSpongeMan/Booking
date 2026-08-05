using Booking.BLL.Exceptions;
using Booking.Shared.Interfaces;
using Booking.Shared.Models;
using System.Threading;

namespace Booking.BLL.Services
{
    public class TableService: ITableService
    {
        private ITableRepository _tableRepo;

        public TableService(ITableRepository tableRepo)
        {
            _tableRepo = tableRepo;
        }

        public async Task<List<Table>> GetAllAsync()
        {
            return await _tableRepo.GetAllAsync();
        }

        public async Task<Table?> GetByNumberAsync(int number)
        {
            var table = await _tableRepo.GetByNumberAsync(number);
            if (table == null)
            {
                throw new NotFoundException($"Столик с номером {number} не найден");
            }

            return table;
        }

        public async Task<List<Table>> GetBySeatsAsync(int seats)
        {
            var tables = await _tableRepo.GetBySeatsAsync(seats);
            if (tables == null)
            {
                throw new NotFoundException($"Столики с числом мест {seats} не найдены");
            }

            return tables;
        }

        public async Task<List<Table>> GetNearFountainAsync()
        {
            var tables = await _tableRepo.GetNearFountainAsync();
            if (tables == null)
            {
                throw new NotFoundException($"Столики около фонтана не найдены");
            }

            return tables;
        }

        public async Task AddAsync(Table table)
        {
            //var tables = await _tableRepo.GetAllAsync();
            //var tableConflict = tables.Where(x => x.Id == table.Id || x.Number == table.Number);
            var tableConflict = await _tableRepo.GetByNumberAsync(table.Number);

            if (tableConflict != null)
            {
                throw new ConflictException($"Столик с номером {table.Number} уже существует");
            }

            await _tableRepo.AddAsync(table);
        }

        public async Task UpdateAsync(Table table)
        {
            var tables = await _tableRepo.GetAllAsync();
            var tableConflict = tables.FirstOrDefault(x => x.Id == table.Id);

            if (tableConflict == null)
            {
                throw new NotFoundException($"Столик с ID {table.Id} не существует");
            }

            await _tableRepo.UpdateAsync(table);
        }

        public async Task DeleteAsync(Guid id)
        {
            var tables = await _tableRepo.GetAllAsync();
            var tableConflict = tables.FirstOrDefault(x => x.Id == id);

            if (tableConflict == null)
            {
                throw new NotFoundException($"Столик с ID {id} не существует");
            }

            await _tableRepo.DeleteAsync(id);
        }
    }
}
