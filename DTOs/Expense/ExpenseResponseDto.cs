namespace Wa7at_ElDr3yah_API.DTOs.Expense
{
    public class ExpenseResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }

        public string? Category { get; set; }
        public string? Notes { get; set; }

        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
