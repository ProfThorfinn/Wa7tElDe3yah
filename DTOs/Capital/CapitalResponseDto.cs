namespace Wa7at_ElDr3yah_API.DTOs.Capital
{
    public class CapitalResponseDto
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime StartDate { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
