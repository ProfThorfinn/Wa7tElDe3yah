using Wa7at_ElDr3yah_API.DTOs.Finance;

namespace Wa7at_ElDr3yah_API.Services.Interfaces
{
    public interface IFinanceService
    {
        Task<FinanceSummaryDto> GetSummaryAsync();

        Task<FinanceSummaryDto> GetSummaryByMonthAsync(int month, int year);
    }
}