using AutoMapper;
using Booking.DTO;
using Booking.Shared.Interfaces;
using Booking.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        private readonly IMapper _mapper;

        public TableController(ITableService tableService, IMapper mapper)
        {
            _tableService = tableService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TableDTO>>> GetAll()
        {
            var tables = await _tableService.GetAllAsync();
            var response = _mapper.Map<List<TableDTO>>(tables);

            return Ok(response);
        }

        [HttpGet("number:{number}")]
        public async Task<ActionResult<TableDTO>> GetByNumber(int number)
        {
            var table = await _tableService.GetByNumberAsync(number);
            var response = _mapper.Map<TableDTO>(table);

            return Ok(response);
        }

        [HttpGet("seats:{seats}")]
        public async Task<ActionResult<List<TableDTO>>> GetBySeats(int seats)
        {
            var tables = await _tableService.GetBySeatsAsync(seats);
            var response = _mapper.Map<List<TableDTO>>(tables);

            return Ok(response);
        }

        [HttpGet("fountain")]
        public async Task<ActionResult<List<TableDTO>>> GetNearFountain()
        {
            var tables = await _tableService.GetNearFountainAsync();
            var response = _mapper.Map<List<TableDTO>>(tables);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTable([FromBody] CreatingTableDTO table)
        {
            var newTable = _mapper.Map<Table>(table);
            await _tableService.AddAsync(newTable);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTable([FromBody] TableDTO table)
        {
            await _tableService.UpdateAsync(_mapper.Map<Table>(table));

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTable(Guid id)
        {
            await _tableService.DeleteAsync(id);

            return Ok();
        }
    }
}
