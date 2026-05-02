using Wa7at_ElDr3yah_API.DTOs.Booking;
using Wa7at_ElDr3yah_API.Models;

namespace Wa7at_ElDr3yah_API.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingResponseDto>> GetAllAsync();

        Task<BookingResponseDto?> GetByIdAsync(int id);

        Task<BookingResponseDto> CreateAsync(BookingRequestDto dto, int userId);

        Task<BookingResponseDto?> UpdateAsync(int id, BookingRequestDto dto);

        Task<bool> DeleteAsync(int id);

        Task<List<DateTime>> GetBookedDatesAsync();

        Task<List<BookingResponseDto>> FilterAsync(
            DateTime? from,
            DateTime? to,
            BookingStatus? status
        );

        Task<List<BookingResponseDto>> SearchAsync(string keyword);
    }
}