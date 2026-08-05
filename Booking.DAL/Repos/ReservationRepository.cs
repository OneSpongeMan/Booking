using Booking.Shared.Interfaces;
using Booking.Shared.Models;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Booking.DAL.Repos
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _semaphore;
        private readonly JsonSerializerOptions _jsonOptions;

        public ReservationRepository(string filePath)
        {
            _filePath = filePath;
            _semaphore = new SemaphoreSlim(1, 1);

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
        }

        private async Task<List<Reservation>> ReadFromFileAsync()
        {
            using (var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (stream.Length == 0) return new List<Reservation>();

                var list = await JsonSerializer.DeserializeAsync<List<Reservation>>(stream, _jsonOptions);
                return list ?? new List<Reservation>();
            }
        }

        private async Task WriteToFileAsync(List<Reservation> reservation)
        {
            using (var stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await JsonSerializer.SerializeAsync(stream, reservation, _jsonOptions);
            }
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                return await ReadFromFileAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<Reservation?> GetByIdAsync(Guid id)
        {
            await _semaphore.WaitAsync();
            try
            {
                var reservations = await ReadFromFileAsync();
                return reservations.FirstOrDefault(x => x.Id == id);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // Находим все брони в нужном диапазоне
        // Если таких нет - возвращаем null
        public async Task<List<Reservation>> GetByTimeAsync(DateTime start, DateTime end)
        {
            await _semaphore.WaitAsync();
            try
            {
                var reservations = await ReadFromFileAsync();
                return reservations.FindAll(x => x.Start >= start && x.End <= end);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<Reservation>> GetByGuestAsync(Guid guestId)
        {
            await _semaphore.WaitAsync();
            try
            {
                var reservation = await ReadFromFileAsync();
                return reservation.FindAll(x => x.Guest == guestId);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<Reservation>> GetByTableAsync(int tableNum)
        {
            await _semaphore.WaitAsync();
            try
            {
                var reservations = await ReadFromFileAsync();
                return reservations.FindAll(x => x.Table == tableNum);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _semaphore.WaitAsync();
            try
            {
                var reservations = await ReadFromFileAsync();

                if (reservation.Id == Guid.Empty)
                    reservation.Id = Guid.NewGuid();
                else if (reservations.FindIndex(x => x.Id == reservation.Id) != -1)
                    throw new Exception($"Бронирование с ID {reservation.Id} уже существует.");

                reservations.Add(reservation);
                await WriteToFileAsync(reservations);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            await _semaphore.WaitAsync();
            try
            {
                var reservations = await ReadFromFileAsync();
                var index = reservations.FindIndex(x => x.Id == reservation.Id);

                if (index != -1)
                {
                    reservations[index] = reservation;
                    await WriteToFileAsync(reservations);
                }
                else
                {
                    throw new KeyNotFoundException($"Бронирование с ID {reservation.Id} не найдено для обновления.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _semaphore.WaitAsync();
            try
            {
                var reservations = await ReadFromFileAsync();
                var reservationToRemove = reservations.FirstOrDefault(x => x.Id == id);

                if (reservationToRemove != null)
                {
                    reservations.Remove(reservationToRemove);
                    await WriteToFileAsync(reservations);
                }
                else
                {
                    throw new KeyNotFoundException($"Бронирование с ID {id} не найдено для удаления.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
