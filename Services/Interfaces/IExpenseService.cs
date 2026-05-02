using Wa7at_ElDr3yah_API.DTOs.Expense;
using Wa7at_ElDr3yah_API.DTOs.Common;

namespace Wa7at_ElDr3yah_API.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<List<ExpenseResponseDto>> GetAllAsync();
        Task<ExpenseResponseDto?> GetByIdAsync(int id);

        Task<ExpenseResponseDto> CreateAsync(ExpenseDto dto, int userId);
        Task<ExpenseResponseDto?> UpdateAsync(int id, ExpenseDto dto);
        Task<bool> DeleteAsync(int id);

        Task<PagedResponseDto<ExpenseResponseDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<List<ExpenseResponseDto>> FilterAsync(
            string? keyword,
            string? category,
            decimal? minAmount,
            decimal? maxAmount,
            DateTime? from,
            DateTime? to
        );

        Task<decimal> GetTotalExpensesAsync();
        Task<decimal> GetTotalExpensesByMonthAsync(int month, int year);

        Task<List<ExpenseResponseDto>> GetByUserAsync(int userId);
        Task<List<ExpenseResponseDto>> GetRecentAsync(int count);
        Task<List<ExpenseResponseDto>> GetTopAsync(int count);
        Task<List<string>> GetCategoriesAsync();
    }
}