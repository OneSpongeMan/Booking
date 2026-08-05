using Booking.Shared.Interfaces;
using Booking.Shared.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Booking.DAL.Repos
{
    public class TableRepository: ITableRepository
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _semaphore;
        private readonly JsonSerializerOptions _jsonOptions;

        public TableRepository(string filePath)
        {
            _filePath = filePath;
            _semaphore = new SemaphoreSlim(1, 1);

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
        }

        private async Task<List<Table>> ReadFromFileAsync()
        {
            using (var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (stream.Length == 0)
                    return new List<Table>();

                var list = await JsonSerializer.DeserializeAsync<List<Table>>(stream, _jsonOptions);
                return list ?? new List<Table>();
            }
        }

        private async Task WriteToFileAsync(List<Table> tables)
        {
            using (var stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await JsonSerializer.SerializeAsync(stream, tables, _jsonOptions);
            }
        }

        public async Task<List<Table>> GetAllAsync()
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

        public async Task<Table?> GetByNumberAsync(int number)
        {
            await _semaphore.WaitAsync();
            try
            {
                var tables = await ReadFromFileAsync();
                return tables.FirstOrDefault(x => x.Number == number);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<Table>> GetBySeatsAsync(int seats)
        {
            await _semaphore.WaitAsync();
            try
            {
                var tables = await ReadFromFileAsync();
                return tables.FindAll(x => x.Seats >= seats);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public async Task<List<Table>> GetNearFountainAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var tables = await ReadFromFileAsync();
                return tables.FindAll(x => x.NearFountain);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public async Task AddAsync(Table table)
        {
            await _semaphore.WaitAsync();
            try
            {
                var tables = await ReadFromFileAsync();

                if (table.Id == Guid.Empty)
                    table.Id = Guid.NewGuid();
                else if (tables.FindIndex(x => x.Id == table.Id) != -1)
                    throw new Exception($"Стол с ID {table.Id} уже существует.");

                tables.Add(table);
                await WriteToFileAsync(tables);
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
        
        public async Task UpdateAsync(Table table)
        {
            await _semaphore.WaitAsync();
            try
            {
                var tables = await ReadFromFileAsync();
                var index = tables.FindIndex(x => x.Id == table.Id);

                if (index != -1)
                {
                    tables[index] = table;
                    await WriteToFileAsync(tables);
                }
                else
                {
                    throw new KeyNotFoundException($"Стол с ID {table.Id} не найден для обновления.");
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
                var tables = await ReadFromFileAsync();
                var tableToRemove = tables.FirstOrDefault(x => x.Id == id);

                if (tableToRemove != null)
                {
                    tables.Remove(tableToRemove);
                    await WriteToFileAsync(tables);
                }
                else
                {
                    throw new KeyNotFoundException($"Столик с ID {id} не найден для удаления.");
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
