using Booking.Shared.Interfaces;
using Booking.Shared.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Booking.DAL.Repos
{
    public class GuestRepository : IGuestRepository
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _semaphore;
        private readonly JsonSerializerOptions _jsonOptions;

        public GuestRepository(string filePath)
        {
            _filePath = filePath;
            _semaphore = new SemaphoreSlim(1, 1);

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
        }

        private async Task<List<Guest>> ReadFromFileAsync()
        {
            using (var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (stream.Length == 0) return new List<Guest>();

                var list = await JsonSerializer.DeserializeAsync<List<Guest>>(stream, _jsonOptions);
                return list ?? new List<Guest>();
            }
        }

        private async Task WriteToFileAsync(List<Guest> guests)
        {
            using (var stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await JsonSerializer.SerializeAsync(stream, guests, _jsonOptions);
            }
        }

        public async Task<List<Guest>> GetAllAsync()
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

        public async Task<Guest?> GetByIdAsync(Guid id)
        {
            await _semaphore.WaitAsync();
            try
            {
                var guests = await ReadFromFileAsync();
                return guests.FirstOrDefault(x => x.Id == id);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<Guest?> GetByPhoneAsync(string phone)
        {
            await _semaphore.WaitAsync();
            try
            {
                var guests = await ReadFromFileAsync();
                return guests.FirstOrDefault(x => x.Phone == phone);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<Guest>> GetByNameAsync(string surname, string name, string patronymic)
        {
            await _semaphore.WaitAsync();
            try
            {
                var guests = await ReadFromFileAsync();
                return guests.FindAll(x => x.Surname == surname && x.Name == name && x.Patronymic == patronymic);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<Guest>> GetRegularCustomersAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var guests = await ReadFromFileAsync();
                return guests.FindAll(x => x.RegularCustomer);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task AddAsync(Guest guest)
        {
            await _semaphore.WaitAsync();
            try
            {
                var guests = await ReadFromFileAsync();

                if (guest.Id == Guid.Empty)
                    guest.Id = Guid.NewGuid();
                else if (guests.FindIndex(x => x.Id == guest.Id) != -1)
                    throw new Exception($"Гость с ID {guest.Id} уже существует.");

                guests.Add(guest);
                await WriteToFileAsync(guests);
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

        public async Task UpdateAsync(Guest guest)
        {
            await _semaphore.WaitAsync();
            try
            {
                var guests = await ReadFromFileAsync();
                var index = guests.FindIndex(x => x.Id == guest.Id);

                if (index != -1)
                {
                    guests[index] = guest;
                    await WriteToFileAsync(guests);
                }
                else
                {
                    throw new KeyNotFoundException($"Гость с ID {guest.Id} не найден для обновления.");
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
                var guests = await ReadFromFileAsync();
                var guestToRemove = guests.FirstOrDefault(x => x.Id == id);

                if (guestToRemove != null)
                {
                    guests.Remove(guestToRemove);
                    await WriteToFileAsync(guests);
                }
                else
                {
                    throw new KeyNotFoundException($"Гость с ID {id} не найден для удаления.");
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
