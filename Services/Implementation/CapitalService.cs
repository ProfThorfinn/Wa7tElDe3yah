using Microsoft.EntityFrameworkCore;
using Wa7at_ElDr3yah_API.Data;
using Wa7at_ElDr3yah_API.DTOs.Capital;
using Wa7at_ElDr3yah_API.Models;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Services.Implementation
{
    public class CapitalService : ICapitalService
    {
        private readonly AppDbContext _context;

        public CapitalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CapitalResponseDto>> GetAllAsync()
        {
            return await _context.Capitals
                .Include(c => c.CreatedByUser)
                .OrderByDescending(c => c.StartDate)
                .Select(c => new CapitalResponseDto
                {
                    Id = c.Id,
                    Amount = c.Amount,
                    StartDate = c.StartDate,
                    Notes = c.Notes,
                    CreatedByUserName = c.CreatedByUser != null ? c.CreatedByUser.FullName : "Unknown",
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<CapitalResponseDto?> GetByIdAsync(int id)
        {
            var capital = await _context.Capitals
                .Include(c => c.CreatedByUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (capital == null)
                return null;

            return MapToResponseDto(capital);
        }

        public async Task<CapitalResponseDto> CreateAsync(CapitalDto dto, int userId)
        {
            if (dto.Amount <= 0)
                throw new Exception("Capital amount must be greater than zero");

            var capital = new Capital
            {
                Amount = dto.Amount,
                StartDate = dto.StartDate,
                Notes = dto.Notes,
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Capitals.Add(capital);
            await _context.SaveChangesAsync();

            var createdCapital = await _context.Capitals
                .Include(c => c.CreatedByUser)
                .FirstAsync(c => c.Id == capital.Id);

            return MapToResponseDto(createdCapital);
        }

        public async Task<CapitalResponseDto?> UpdateAsync(int id, CapitalDto dto)
        {
            var capital = await _context.Capitals
                .Include(c => c.CreatedByUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (capital == null)
                return null;

            if (dto.Amount <= 0)
                throw new Exception("Capital amount must be greater than zero");

            capital.Amount = dto.Amount;
            capital.StartDate = dto.StartDate;
            capital.Notes = dto.Notes;

            await _context.SaveChangesAsync();

            return MapToResponseDto(capital);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var capital = await _context.Capitals.FindAsync(id);

            if (capital == null)
                return false;

            _context.Capitals.Remove(capital);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<decimal> GetTotalCapitalAsync()
        {
            return await _context.Capitals
                .SumAsync(c => (decimal?)c.Amount) ?? 0;
        }

        public async Task<CapitalResponseDto?> GetLatestCapitalAsync()
        {
            var capital = await _context.Capitals
                .Include(c => c.CreatedByUser)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (capital == null)
                return null;

            return MapToResponseDto(capital);
        }

        private static CapitalResponseDto MapToResponseDto(Capital capital)
        {
            return new CapitalResponseDto
            {
                Id = capital.Id,
                Amount = capital.Amount,
                StartDate = capital.StartDate,
                Notes = capital.Notes,
                CreatedByUserName = capital.CreatedByUser != null ? capital.CreatedByUser.FullName : "Unknown",
                CreatedAt = capital.CreatedAt
            };
        }
    }
}