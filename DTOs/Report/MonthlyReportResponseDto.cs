namespace Wa7at_ElDr3yah_API.DTOs.Report
{
    public class MonthlyReportResponseDto
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetProfitOrLoss { get; set; }
        public decimal ProfitLossPercentage { get; set; }
        public string? PdfFilePath { get; set; }
        public string GeneratedByUserName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
    }
}