using Wa7at_ElDr3yah_API.Models;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.Admin;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? ResetPasswordCodeHash { get; set; }
    public DateTime? ResetPasswordCodeExpiresAt { get; set; }
    public DateTime? LastResetPasswordRequestAt { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Capital> Capitals { get; set; } = new List<Capital>();
    public ICollection<MonthlyReport> MonthlyReports { get; set; } = new List<MonthlyReport>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}