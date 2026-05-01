using Wa7at_ElDr3yah_API.Models;

public class AuditLog
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string EntityName { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public AuditAction Action { get; set; }

    public string? Details { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}