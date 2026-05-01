using System.ComponentModel.DataAnnotations;

namespace Wa7at_ElDr3yah_API.DTOs.Expense
{
    public class ExpenseDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        public string? Notes { get; set; }
    }
}
