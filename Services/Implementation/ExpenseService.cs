using Microsoft.EntityFrameworkCore;
using Wa7at_ElDr3yah_API.Data;
using Wa7at_ElDr3yah_API.DTOs.Common;
using Wa7at_ElDr3yah_API.DTOs.Expense;
using Wa7at_ElDr3yah_API.Models;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Services.Implementation
{
    public class ExpenseService : IExpenseService
    {
        private readonly AppDbContext _context;

        public ExpenseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseResponseDto>> GetAllAsync()
        {
            return await _context.Expenses
                .Include(e => e.CreatedByUser)
                .OrderByDescending(e => e.ExpenseDate)
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    ExpenseDate = e.ExpenseDate,
                    Category = e.Category,
                    Notes = e.Notes,
                    CreatedByUserName = e.CreatedByUser != null ? e.CreatedByUser.FullName : "Unknown",
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ExpenseResponseDto?> GetByIdAsync(int id)
        {
            var expense = await _context.Expenses
                .Include(e => e.CreatedByUser)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null)
                return null;

            return MapToResponseDto(expense);
        }

        public async Task<ExpenseResponseDto> CreateAsync(ExpenseDto dto, int userId)
        {
            if (dto.Amount <= 0)
                throw new Exception("Expense amount must be greater than zero");

            var expense = new Expense
            {
                Title = dto.Title,
                Amount = dto.Amount,
                ExpenseDate = dto.ExpenseDate,
                Category = dto.Category,
                Notes = dto.Notes,
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var createdExpense = await _context.Expenses
                .Include(e => e.CreatedByUser)
                .FirstAsync(e => e.Id == expense.Id);

            return MapToResponseDto(createdExpense);
        }

        public async Task<ExpenseResponseDto?> UpdateAsync(int id, ExpenseDto dto)
        {
            var expense = await _context.Expenses
                .Include(e => e.CreatedByUser)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null)
                return null;

            if (dto.Amount <= 0)
                throw new Exception("Expense amount must be greater than zero");

            expense.Title = dto.Title;
            expense.Amount = dto.Amount;
            expense.ExpenseDate = dto.ExpenseDate;
            expense.Category = dto.Category;
            expense.Notes = dto.Notes;

            await _context.SaveChangesAsync();

            return MapToResponseDto(expense);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
                return false;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ExpenseResponseDto>> FilterAsync(
            string? keyword,
            string? category,
            decimal? minAmount,
            decimal? maxAmount,
            DateTime? from,
            DateTime? to)
        {
            var query = _context.Expenses
                .Include(e => e.CreatedByUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(e =>
                    e.Title.Contains(keyword) ||
                    (e.Notes != null && e.Notes.Contains(keyword)));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category == category);
            }

            if (minAmount.HasValue)
            {
                query = query.Where(e => e.Amount >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                query = query.Where(e => e.Amount <= maxAmount.Value);
            }

            if (from.HasValue)
            {
                query = query.Where(e => e.ExpenseDate.Date >= from.Value.Date);
            }

            if (to.HasValue)
            {
                query = query.Where(e => e.ExpenseDate.Date <= to.Value.Date);
            }

            return await query
                .OrderByDescending(e => e.ExpenseDate)
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    ExpenseDate = e.ExpenseDate,
                    Category = e.Category,
                    Notes = e.Notes,
                    CreatedByUserName = e.CreatedByUser != null ? e.CreatedByUser.FullName : "Unknown",
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<decimal> GetTotalExpensesAsync()
        {
            return await _context.Expenses
                .SumAsync(e => e.Amount);
        }

        public async Task<decimal> GetTotalExpensesByMonthAsync(int month, int year)
        {
            return await _context.Expenses
                .Where(e => e.ExpenseDate.Month == month && e.ExpenseDate.Year == year)
                .SumAsync(e => e.Amount);
        }

        public async Task<List<ExpenseResponseDto>> GetByUserAsync(int userId)
        {
            return await _context.Expenses
                .Where(e => e.CreatedByUserId == userId)
                .Include(e => e.CreatedByUser)
                .OrderByDescending(e => e.ExpenseDate)
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    ExpenseDate = e.ExpenseDate,
                    Category = e.Category,
                    Notes = e.Notes,
                    CreatedByUserName = e.CreatedByUser != null ? e.CreatedByUser.FullName : "Unknown",
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<List<ExpenseResponseDto>> GetRecentAsync(int count)
        {
            return await _context.Expenses
                .Include(e => e.CreatedByUser)
                .OrderByDescending(e => e.CreatedAt)
                .Take(count)
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    ExpenseDate = e.ExpenseDate,
                    Category = e.Category,
                    Notes = e.Notes,
                    CreatedByUserName = e.CreatedByUser != null ? e.CreatedByUser.FullName : "Unknown",
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<List<ExpenseResponseDto>> GetTopAsync(int count)
        {
            return await _context.Expenses
                .Include(e => e.CreatedByUser)
                .OrderByDescending(e => e.Amount)
                .Take(count)
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    ExpenseDate = e.ExpenseDate,
                    Category = e.Category,
                    Notes = e.Notes,
                    CreatedByUserName = e.CreatedByUser != null ? e.CreatedByUser.FullName : "Unknown",
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.Expenses
                .Where(e => e.Category != null && e.Category != "")
                .Select(e => e.Category!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<PagedResponseDto<ExpenseResponseDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                pageNumber = 1;

            if (pageSize <= 0)
                pageSize = 10;

            var query = _context.Expenses
                .Include(e => e.CreatedByUser)
                .OrderByDescending(e => e.ExpenseDate)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    ExpenseDate = e.ExpenseDate,
                    Category = e.Category,
                    Notes = e.Notes,
                    CreatedByUserName = e.CreatedByUser != null ? e.CreatedByUser.FullName : "Unknown",
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            return new PagedResponseDto<ExpenseResponseDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = data
            };
        }

        private static ExpenseResponseDto MapToResponseDto(Expense expense)
        {
            return new ExpenseResponseDto
            {
                Id = expense.Id,
                Title = expense.Title,
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                Category = expense.Category,
                Notes = expense.Notes,
                CreatedByUserName = expense.CreatedByUser != null ? expense.CreatedByUser.FullName : "Unknown",
                CreatedAt = expense.CreatedAt
            };
        }
    }
}