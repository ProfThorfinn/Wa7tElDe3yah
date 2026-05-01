using System.ComponentModel.DataAnnotations;

namespace Wa7at_ElDr3yah_API.DTOs.Capital
{
    public class CapitalDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public string? Notes { get; set; }
    }
}
