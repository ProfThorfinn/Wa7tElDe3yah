namespace Wa7at_ElDr3yah_API.DTOs.Booking
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;

        public DateTime BookingDate { get; set; }
        public string DayName { get; set; } = string.Empty;

        public string BookingType { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public string CreatedByUserName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}