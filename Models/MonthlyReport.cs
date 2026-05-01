using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wa7at_ElDr3yah_API.Models
{
    public class MonthlyReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int Year { get; set; }

        [Required]
        public decimal TotalRevenue { get; set; }

        [Required]
        public decimal TotalExpenses { get; set; }

        public decimal NetProfitOrLoss { get; set; }

        public decimal ProfitLossPercentage { get; set; }

        [MaxLength(500)]
        public string? PdfFilePath { get; set; }

        [Required]
        public int GeneratedByUserId { get; set; }

        [ForeignKey(nameof(GeneratedByUserId))]
        public User GeneratedByUser { get; set; } = null!;

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}