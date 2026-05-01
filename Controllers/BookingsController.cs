using Microsoft.AspNetCore.Mvc;
using Wa7at_ElDr3yah_API.DTOs.Booking;
using Wa7at_ElDr3yah_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Wa7at_ElDr3yah_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _bookingService.GetAllAsync();
            return Ok(data);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found");

            return Ok(booking);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingRequestDto dto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("uid")!.Value);

                var result = await _bookingService.CreateAsync(dto, userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookingRequestDto dto)
        {
            try
            {
                var result = await _bookingService.UpdateAsync(id, dto);

                if (result == null)
                    return NotFound("Booking not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
       [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _bookingService.DeleteAsync(id);

            if (!deleted)
                return NotFound("Booking not found");

            return Ok("Deleted successfully");
        }

        
        [HttpGet("booked-dates")]
        public async Task<IActionResult> GetBookedDates()
        {
            var dates = await _bookingService.GetBookedDatesAsync();
            return Ok(dates);
        }
    }
}