using Wa7at_ElDr3yah_API.DTOs.Report;

namespace Wa7at_ElDr3yah_API.Services.Interfaces
{
    public interface IMonthlyReportService
    {
        Task<List<MonthlyReportResponseDto>> GetAllAsync();
        Task<MonthlyReportResponseDto?> GetByIdAsync(int id);
        Task<MonthlyReportResponseDto> GenerateMonthlyReportAsync(int month, int year, int userId);
        Task<byte[]> ExportMonthlyReportPdfAsync(int month, int year);
    }
}