using AutoMapper;
using Booking.DTO;
using Booking.Shared.Interfaces;
using Booking.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;

        public ReservationController(IReservationService reservationService, IMapper mapper)
        {
            _reservationService = reservationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservationDTO>>> GetAll()
        {
            var reservations = await _reservationService.GetAllAsync();
            var response = _mapper.Map<List<ReservationDTO>>(reservations);
            
            return Ok(response);
        }

        [HttpGet("id:{id}")]
        public async Task<ActionResult<ReservationDTO>> GetById(Guid id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            var response = _mapper.Map<ReservationDTO>(reservation);
            
            return Ok(response);
        }

        [HttpGet("available_tables/start={start}&end={end}")]
        public async Task<ActionResult<List<TableDTO>>> GetAvailableTables(DateTime start, DateTime end)
        {
            var tables = await _reservationService.GetAvailableTablesAsync(start, end);
            var response = _mapper.Map<List<TableDTO>>(tables);

            return Ok(response);
        }

        [HttpGet("guestID:{guestId}")]
        public async Task<ActionResult<List<ReservationDTO>>> GetByGuest(Guid guestId)
        {
            var reservations = await _reservationService.GetByGuestAsync(guestId);
            var response = _mapper.Map<List<ReservationDTO>>(reservations);

            return Ok(response);
        }

        [HttpGet("table:{table}")]
        public async Task<ActionResult<List<ReservationDTO>>> GetByTable(int table)
        {
            var reservations = await _reservationService.GetByTableAsync(table);
            var response = _mapper.Map<List<ReservationDTO>>(reservations);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateReservation([FromBody] CreatingReservationDTO reservation)
        {
            var newReservation = _mapper.Map<Reservation>(reservation);
            await _reservationService.AddAsync(newReservation);

            return Ok();

            //var response = _mapper.Map<ReservationDTO>(await _reservationService.GetBy);
            //var newEntity = _mapper.Map<BookingEntity>(dto);
            //var createdBooking = await _bookingService.CreateAsync(newEntity);
            //// Превращаем созданную сущность обратно в DTO для ответа
            //var responseDto = _mapper.Map<BookingResponseDto>(createdBooking);
            //return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateReservation([FromBody] ReservationDTO reservation)
        {
            await _reservationService.UpdateAsync(_mapper.Map<Reservation>(reservation));

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReservation(Guid id)
        {
            await _reservationService.DeleteAsync(id);

            return Ok();
        }
    }
}
