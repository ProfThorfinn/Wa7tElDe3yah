using Microsoft.EntityFrameworkCore;
using Wa7at_ElDr3yah_API.Data;
using Wa7at_ElDr3yah_API.DTOs.Finance;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Services.Implementation
{
    public class FinanceService : IFinanceService
    {
        private readonly AppDbContext _context;

        public FinanceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FinanceSummaryDto> GetSummaryAsync()
        {
            var totalCapital = await _context.Capitals
                .SumAsync(c => (decimal?)c.Amount) ?? 0;

            var totalRevenue = await _context.Bookings
                .SumAsync(b => (decimal?)b.PaidAmount) ?? 0;

            var totalExpenses = await _context.Expenses
                .SumAsync(e => (decimal?)e.Amount) ?? 0;

            var availableBalance = totalCapital + totalRevenue - totalExpenses;

            var netProfit = totalRevenue - totalExpenses;

            var profitPercentage = totalExpenses == 0
                ? 0
                : (netProfit / totalExpenses) * 100;

            return new FinanceSummaryDto
            {
                TotalCapital = totalCapital,
                TotalRevenue = totalRevenue,
                TotalExpenses = totalExpenses,
                AvailableBalance = availableBalance,
                NetProfitOrLoss = netProfit,
                ProfitLossPercentage = profitPercentage
            };
        }

        public async Task<FinanceSummaryDto> GetSummaryByMonthAsync(int month, int year)
        {
            var totalCapital = await _context.Capitals
                .SumAsync(c => (decimal?)c.Amount) ?? 0;

            var totalRevenue = await _context.Bookings
                .Where(b => b.BookingDate.Month == month && b.BookingDate.Year == year)
                .SumAsync(b => (decimal?)b.PaidAmount) ?? 0;

            var totalExpenses = await _context.Expenses
                .Where(e => e.ExpenseDate.Month == month && e.ExpenseDate.Year == year)
                .SumAsync(e => (decimal?)e.Amount) ?? 0;

            var availableBalance = totalCapital + totalRevenue - totalExpenses;

            var netProfit = totalRevenue - totalExpenses;

            var profitPercentage = totalExpenses == 0
                ? 0
                : (netProfit / totalExpenses) * 100;

            return new FinanceSummaryDto
            {
                TotalCapital = totalCapital,
                TotalRevenue = totalRevenue,
                TotalExpenses = totalExpenses,
                AvailableBalance = availableBalance,
                NetProfitOrLoss = netProfit,
                ProfitLossPercentage = profitPercentage
            };
        }
    }
}