using Booking.BLL.Exceptions;
using Booking.Shared.Interfaces;
using Booking.Shared.Models;
using System.Threading;

namespace Booking.BLL.Services
{
    public class ReservationService : IReservationService
    {
        private IReservationRepository _reservationRepo;
        private ITableRepository _tableRepository;

        public ReservationService(IReservationRepository reservationRepo, ITableRepository tableRepository)
        {
            this._reservationRepo = reservationRepo;
            this._tableRepository = tableRepository;
        }


        public async Task<List<Reservation>> GetAllAsync()
        {
            return await _reservationRepo.GetAllAsync();
        }

        public async Task<Reservation?> GetByIdAsync(Guid id)
        {
            var reservation = await _reservationRepo.GetByIdAsync(id);
            if (reservation == null)
            {
                throw new NotFoundException($"Бронь с ID {id} не найдена");
            }

            return reservation;
        }

        // Где может находиться этот метод: в репе резерва или столов
        public async Task<List<Table>> GetAvailableTablesAsync(DateTime start, DateTime end)
        {
            var reservations = await _reservationRepo.GetByTimeAsync(start, end);
            var tables = await _tableRepository.GetAllAsync();

            var bookedTables = reservations.Select(x => x.Table).ToHashSet();
            var availableTables = tables.FindAll(x => !bookedTables.Contains(x.Number));

            if (availableTables == null)
            {
                throw new NotFoundException($"Доступных столиков на диапазон с {start} до {end} не найдено");
            }

            return availableTables;
        }

        public async Task<List<Reservation>> GetByGuestAsync(Guid guestId)
        {
            var reservations = await _reservationRepo.GetByGuestAsync(guestId);
            if (reservations == null)
            {
                throw new NotFoundException($"Клиент с ID {guestId} не бронировал столики");
            }

            return reservations;
        }

        public async Task<List<Reservation>> GetByTableAsync(int table)
        {
            var reservations = await _reservationRepo.GetByTableAsync(table);
            if (reservations == null)
            {
                throw new NotFoundException($"Стол с номером {table} не бронировался");
            }

            return reservations;
        }

        public async Task AddAsync(Reservation reservation)
        {
            var reservations = await _reservationRepo.GetAllAsync();
            var reservatonConflict = reservations
                .Where(x => x.Table == reservation.Table)
                .Any(x => CheckOverlap(x.Start, x.End, reservation.Start, reservation.End));

            if (reservatonConflict)
            {
                throw new ConflictException("Этот столик уже забронирован на выбранное время");
            }

            await _reservationRepo.AddAsync(reservation);
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            var reservationToUpdate = await _reservationRepo.GetByIdAsync(reservation.Id);
            if (reservationToUpdate == null)
                throw new NotFoundException($"Бронь с ID {reservation.Id} не найдена для изменения");

            var reservations = await _reservationRepo.GetAllAsync();

            bool reservatonConflict = reservations
                .Where(x => x.Table == reservation.Table && x.Id != reservation.Id)
                .Any(x => CheckOverlap(x.Start, x.End, reservation.Start, reservation.End));

            if (reservatonConflict)
            {
                throw new ConflictException("Измененная бронь пересекается с другой бронью этого столика");
            }

            await _reservationRepo.UpdateAsync(reservation);
        }

        public async Task DeleteAsync(Guid id)
        {
            var reservationToUpdate = await _reservationRepo.GetByIdAsync(id);
            if (reservationToUpdate == null)
                throw new NotFoundException($"Бронь с ID {id} не найдена для удаления");

            await _reservationRepo.DeleteAsync(id);
        }

        private bool CheckOverlap(DateTime startOld, DateTime endOld, DateTime startNew, DateTime endNew)
        {
            return endNew <= startOld || startNew >= endOld;
        }
    }
}
