using Wa7at_ElDr3yah_API.DTOs.Capital;

namespace Wa7at_ElDr3yah_API.Services.Interfaces
{
    public interface ICapitalService
    {
        Task<List<CapitalResponseDto>> GetAllAsync();

        Task<CapitalResponseDto?> GetByIdAsync(int id);

        Task<CapitalResponseDto> CreateAsync(CapitalDto dto, int userId);

        Task<CapitalResponseDto?> UpdateAsync(int id, CapitalDto dto);

        Task<bool> DeleteAsync(int id);

        Task<decimal> GetTotalCapitalAsync();

        Task<CapitalResponseDto?> GetLatestCapitalAsync();
    }
}