using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wa7at_ElDr3yah_API.DTOs.Booking;
using Wa7at_ElDr3yah_API.Models;
using Wa7at_ElDr3yah_API.Services.Interfaces;

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

        /// <summary>
        /// Get all bookings.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _bookingService.GetAllAsync();
            return Ok(data);
        }

        /// <summary>
        /// Get booking by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found");

            return Ok(booking);
        }

        /// <summary>
        /// Create a new booking.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingRequestDto dto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("uid")!.Value);

                var result = await _bookingService.CreateAsync(dto, userId);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing booking.
        /// </summary>
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

        /// <summary>
        /// Delete a booking.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _bookingService.DeleteAsync(id);

            if (!deleted)
                return NotFound("Booking not found");

            return Ok("Deleted successfully");
        }

        /// <summary>
        /// Get booked dates.
        /// </summary>
        [HttpGet("booked-dates")]
        public async Task<IActionResult> GetBookedDates()
        {
            var dates = await _bookingService.GetBookedDatesAsync();
            return Ok(dates);
        }

        /// <summary>
        /// Filter bookings by date range and status.
        /// </summary>
        /// <remarks>
        /// Example:
        /// GET /api/bookings/filter?from=2026-05-01&amp;to=2026-05-31&amp;status=Booked
        /// </remarks>
        [HttpGet("filter")]
        public async Task<IActionResult> Filter(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] BookingStatus? status)
        {
            var result = await _bookingService.FilterAsync(from, to, status);
            return Ok(result);
        }

        /// <summary>
        /// Search bookings by customer name or phone number.
        /// </summary>
        /// <remarks>
        /// Example:
        /// GET /api/bookings/search?keyword=Mahmoud
        /// GET /api/bookings/search?keyword=0555
        /// </remarks>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var result = await _bookingService.SearchAsync(keyword);
            return Ok(result);
        }
    }
}