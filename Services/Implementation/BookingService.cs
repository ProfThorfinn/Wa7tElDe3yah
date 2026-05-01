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
                .OrderBy(b => b.BookingDate)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    CustomerName = b.CustomerName,
                    ContactNumber = b.ContactNumber,
                    BookingDate = b.BookingDate,
                    DayName = b.DayName,
                    BookingType = b.BookingType,
                    TotalPrice = b.TotalPrice,
                    PaidAmount = b.PaidAmount,
                    RemainingAmount = b.TotalPrice - b.PaidAmount,
                    Status = b.Status.ToString(),
                    Notes = b.Notes,
                    CreatedByUserName = b.CreatedByUser.FullName,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<BookingResponseDto?> GetByIdAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.CreatedByUser)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return null;

            return MapToResponseDto(booking);
        }

        public async Task<BookingResponseDto> CreateAsync(BookingRequestDto dto, int userId)
        {
            var isBooked = await _context.Bookings
                .AnyAsync(b => b.BookingDate.Date == dto.BookingDate.Date);

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
                CreatedByUserId = userId
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

            var isBooked = await _context.Bookings
                .AnyAsync(b => b.Id != id && b.BookingDate.Date == dto.BookingDate.Date);

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