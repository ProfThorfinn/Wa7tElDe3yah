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

        /// <summary>
        /// Get all bookings.
        /// </summary>
        /// <remarks>
        /// Returns all bookings ordered by booking date.
        /// Each booking includes customer data, payment details, remaining amount, status, and creator information.
        /// Requires authentication.
        /// </remarks>
        /// <returns>List of bookings.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _bookingService.GetAllAsync();
            return Ok(data);
        }

        /// <summary>
        /// Get booking by id.
        /// </summary>
        /// <remarks>
        /// Returns a single booking using its unique identifier.
        /// Requires authentication.
        /// </remarks>
        /// <param name="id">Booking id.</param>
        /// <returns>Booking details if found.</returns>
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
        /// <remarks>
        /// Creates a new booking with the provided details.
        /// Requires authentication.
        /// </remarks>
        /// <param name="dto">Booking details.</param>
        /// <returns>Created booking details.</returns>
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

        /// <summary>
        /// Update an existing booking.
        /// </summary>
        /// <remarks>
        /// Business rules:
        /// - User must be authenticated.
        /// - Booking must exist.
        /// - The updated date cannot conflict with another booking.
        /// - PaidAmount cannot be greater than TotalPrice.
        /// - RemainingAmount is recalculated automatically.
        /// </remarks>
        /// <param name="id">Booking id.</param>
        /// <param name="dto">Updated booking data.</param>
        /// <returns>The updated booking details.</returns>
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
        /// <remarks>
        /// Deletes an existing booking by id.
        /// Requires authentication.
        /// </remarks>
        /// <param name="id">Booking id.</param>
        /// <returns>Success message if deleted.</returns>
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
        /// <remarks>
        /// Returns all reserved dates.
        /// Used by the frontend calendar to highlight booked dates and prevent double booking.
        /// Requires authentication.
        /// </remarks>
        /// <returns>List of booked dates.</returns>
        [HttpGet("booked-dates")]
        public async Task<IActionResult> GetBookedDates()
        {
            var dates = await _bookingService.GetBookedDatesAsync();
            return Ok(dates);
        }
    }
}