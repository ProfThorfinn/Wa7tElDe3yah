using Wa7at_ElDr3yah_API.DTOs.User;

namespace Wa7at_ElDr3yah_API.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<UserResponseDto> CreateAsync(UserRequestDto dto);
        Task<UserResponseDto?> UpdateAsync(int id, UserRequestDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleStatusAsync(int id);
    }
}