using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wa7at_ElDr3yah_API.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string ContactNumber { get; set; } = string.Empty;

        [Required]
        public DateTime BookingDate { get; set; }

        [Required, MaxLength(20)]
        public string DayName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string BookingType { get; set; } = string.Empty;

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public decimal PaidAmount { get; set; }

        [Required]
        public decimal RemainingAmount { get; set; }

        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Booked;

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public User CreatedByUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}