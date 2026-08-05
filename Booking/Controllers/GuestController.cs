using AutoMapper;
using Booking.DTO;
using Booking.Shared.Interfaces;
using Booking.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly IGuestService _guestService;
        private readonly IMapper _mapper;

        public GuestController(IGuestService guestService, IMapper mapper)
        {
            _guestService = guestService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GuestDTO>>> GetAll()
        {
            var guests = await _guestService.GetAllAsync();
            var response = _mapper.Map<List<GuestDTO>>(guests);

            return Ok(response);
        }

        [HttpGet("id:{id}")]
        public async Task<ActionResult<GuestDTO>> GetById(Guid id)
        {
            var guest = await _guestService.GetByIdAsync(id);
            var response = _mapper.Map<GuestDTO>(guest);

            return Ok(response);
        }

        [HttpGet("phone:{phone}")]
        public async Task<ActionResult<GuestDTO>> GetByPhone(string phone)
        {
            var guest = await _guestService.GetByPhoneAsync(phone);
            var response = _mapper.Map<GuestDTO>(guest);

            return Ok(response);
        }

        [HttpGet("fullname:{name}")]
        public async Task<ActionResult<List<GuestDTO>>> GetByName(string name)
        {
            var guests = await _guestService.GetByNameAsync(name);
            var response = _mapper.Map<List<GuestDTO>>(guests);

            return Ok(response);
        }

        [HttpGet("regular")]
        public async Task<ActionResult<List<GuestDTO>>> GetRegularCustomers()
        {
            var guests = await _guestService.GetRegularCustomersAsync();
            var response = _mapper.Map<List<GuestDTO>>(guests);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateGuest([FromBody] CreatingGuestDTO guest)
        {
            var newGuest = _mapper.Map<Guest>(guest);
            await _guestService.AddAsync(newGuest);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateGuest([FromBody] GuestDTO guest)
        {
            await _guestService.UpdateAsync(_mapper.Map<Guest>(guest));

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGuest(Guid id)
        {
            await _guestService.DeleteAsync(id);

            return Ok();
        }
    }
}
