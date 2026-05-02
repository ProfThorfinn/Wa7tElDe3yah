using System.ComponentModel.DataAnnotations;

namespace Wa7at_ElDr3yah_API.DTOs.Report
{
    public class MonthlyReportRequestDto
    {
        [Required, Range(1, 12)]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }
    }
}