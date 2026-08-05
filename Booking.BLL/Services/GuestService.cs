using Booking.Shared.Interfaces;
using Booking.Shared.Models;
using Booking.BLL.Exceptions;

namespace Booking.BLL.Services
{
    public class GuestService : IGuestService
    {
        private IGuestRepository _guestRepo;

        public GuestService(IGuestRepository guestRepo)
        {
            this._guestRepo = guestRepo;
        }


        public async Task<List<Guest>> GetAllAsync()
        {
            return await _guestRepo.GetAllAsync();
        }

        public async Task<Guest?> GetByIdAsync(Guid id)
        {
            var guest = await _guestRepo.GetByIdAsync(id);
            if (guest == null)
            {
                throw new NotFoundException($"Гость с ID {id} не найден");
            }

            return guest;
        }

        public async Task<Guest?> GetByPhoneAsync(string phone)
        {
            var guest = await _guestRepo.GetByPhoneAsync(phone);
            if (guest == null)
            {
                throw new NotFoundException($"Гость с номером телефона {phone} не найден");
            }

            return guest;
        }

        public async Task<List<Guest>> GetByNameAsync(string fullName)
        {
            var surname = fullName.Split(' ')[0];
            var name = fullName.Split(' ')[1];
            var patronymic = fullName.Split(' ')[2];

            var guests = await _guestRepo.GetByNameAsync(surname, name, patronymic);
            if (guests == null)
            {
                throw new NotFoundException($"Гости с именем {fullName} не найдены");
            }

            return guests;
        }

        public async Task<List<Guest>> GetRegularCustomersAsync()
        {
            var guests = await _guestRepo.GetRegularCustomersAsync();
            if (guests == null)
            {
                throw new NotFoundException($"Регулярные гости не найден");
            }

            return guests;
        }

        public async Task AddAsync(Guest guest)
        {
            var guestConflict = await _guestRepo.GetByPhoneAsync(guest.Phone);
            if (guestConflict != null)
            {
                throw new ConflictException($"Клиент с номером телефона {guest.Phone} уже существует");
            }

            await _guestRepo.AddAsync(guest);
        }

        public async Task UpdateAsync(Guest guest)
        {
            var guestConflict = await _guestRepo.GetByIdAsync(guest.Id);
            if (guestConflict == null)
            {
                throw new NotFoundException($"Клиента с ID {guest.Id} не существует");
            }

            await _guestRepo.UpdateAsync(guest);
        }

        public async Task DeleteAsync(Guid id)
        {
            var guestConflict = await _guestRepo.GetByIdAsync(id);

            if (guestConflict == null)
            {
                throw new NotFoundException($"Клиента с ID {id} не существует");
            }

            await _guestRepo.DeleteAsync(id);
        }
    }
}
