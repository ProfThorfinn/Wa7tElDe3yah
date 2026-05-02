using Microsoft.EntityFrameworkCore;
using Wa7at_ElDr3yah_API.Data;
using Wa7at_ElDr3yah_API.DTOs.Booking;
using Wa7at_ElDr3yah_API.Models;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookingResponseDto>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.CreatedByUser)
                .OrderByDescending(b => b.BookingDate)
                .Select(b => MapToResponseDto(b))
                .ToListAsync();
        }

        public async Task<BookingResponseDto?> GetByIdAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.CreatedByUser)
                .FirstOrDefaultAsync(b => b.Id == id);

            return booking == null ? null : MapToResponseDto(booking);
        }

        public async Task<BookingResponseDto> CreateAsync(BookingRequestDto dto, int userId)
        {
            var isBooked = await _context.Bookings.AnyAsync(b =>
                b.Status != BookingStatus.Cancelled &&
                b.BookingDate.Date == dto.BookingDate.Date);

            if (isBooked)
                throw new Exception("This date is already booked");

            if (dto.PaidAmount > dto.TotalPrice)
                throw new Exception("Paid amount cannot be greater than total price");

            var booking = new Booking
            {
                CustomerName = dto.CustomerName,
                ContactNumber = dto.ContactNumber,
                BookingDate = dto.BookingDate,
                DayName = dto.DayName,
                BookingType = dto.BookingType,
                TotalPrice = dto.TotalPrice,
                PaidAmount = dto.PaidAmount,
                RemainingAmount = dto.TotalPrice - dto.PaidAmount,
                Notes = dto.Notes,
                Status = BookingStatus.Booked,
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var createdBooking = await _context.Bookings
                .Include(b => b.CreatedByUser)
                .FirstAsync(b => b.Id == booking.Id);

            return MapToResponseDto(createdBooking);
        }

        public async Task<BookingResponseDto?> UpdateAsync(int id, BookingRequestDto dto)
        {
            var booking = await _context.Bookings
                .Include(b => b.CreatedByUser)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return null;

            var isBooked = await _context.Bookings.AnyAsync(b =>
                b.Id != id &&
                b.Status != BookingStatus.Cancelled &&
                b.BookingDate.Date == dto.BookingDate.Date);

            if (isBooked)
                throw new Exception("This date is already booked");

            if (dto.PaidAmount > dto.TotalPrice)
                throw new Exception("Paid amount cannot be greater than total price");

            booking.CustomerName = dto.CustomerName;
            booking.ContactNumber = dto.ContactNumber;
            booking.BookingDate = dto.BookingDate;
            booking.DayName = dto.DayName;
            booking.BookingType = dto.BookingType;
            booking.TotalPrice = dto.TotalPrice;
            booking.PaidAmount = dto.PaidAmount;
            booking.RemainingAmount = dto.TotalPrice - dto.PaidAmount;
            booking.Notes = dto.Notes;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToResponseDto(booking);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<DateTime>> GetBookedDatesAsync()
        {
            return await _context.Bookings
                .Where(b => b.Status != BookingStatus.Cancelled)
                .Select(b => b.BookingDate.Date)
                .ToListAsync();
        }

        public async Task<List<BookingResponseDto>> FilterAsync(
            DateTime? from,
            DateTime? to,
            BookingStatus? status)
        {
            var query = _context.Bookings
                .Include(b => b.CreatedByUser)
                .AsQueryable();

            if (from.HasValue)
                query = query.Where(b => b.BookingDate.Date >= from.Value.Date);

            if (to.HasValue)
                query = query.Where(b => b.BookingDate.Date <= to.Value.Date);

            if (status.HasValue)
                query = query.Where(b => b.Status == status.Value);

            return await query
                .OrderByDescending(b => b.BookingDate)
                .Select(b => MapToResponseDto(b))
                .ToListAsync();
        }

        public async Task<List<BookingResponseDto>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<BookingResponseDto>();

            keyword = keyword.Trim();

            return await _context.Bookings
                .Include(b => b.CreatedByUser)
                .Where(b =>
                    b.CustomerName.Contains(keyword) ||
                    b.ContactNumber.Contains(keyword))
                .OrderByDescending(b => b.BookingDate)
                .Select(b => MapToResponseDto(b))
                .ToListAsync();
        }

        private static BookingResponseDto MapToResponseDto(Booking booking)
        {
            return new BookingResponseDto
            {
                Id = booking.Id,
                CustomerName = booking.CustomerName,
                ContactNumber = booking.ContactNumber,
                BookingDate = booking.BookingDate,
                DayName = booking.DayName,
                BookingType = booking.BookingType,
                TotalPrice = booking.TotalPrice,
                PaidAmount = booking.PaidAmount,
                RemainingAmount = booking.TotalPrice - booking.PaidAmount,
                Status = booking.Status.ToString(),
                Notes = booking.Notes,
                CreatedByUserName = booking.CreatedByUser.FullName,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}