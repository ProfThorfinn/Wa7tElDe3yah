namespace Wa7at_ElDr3yah_API.DTOs.Finance
{

    public class FinanceSummaryDto
    {
        public decimal TotalCapital { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }

        public decimal AvailableBalance { get; set; }

        public decimal NetProfitOrLoss { get; set; }
        public decimal ProfitLossPercentage { get; set; }
    }
}