using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wa7at_ElDr3yah_API.Models;
namespace Wa7at_ElDr3yah_API.Configuration
{
    public class ExpenseConfiguration: IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Amount)
                .IsRequired()
                .HasPrecision(10, 2);
            builder.Property(e => e.ExpenseDate)
                .IsRequired();
            builder.Property(e => e.Category)
                .HasMaxLength(100);
            builder.Property(e => e.Notes)
                .HasMaxLength(500);
            builder.Property(e => e.CreatedByUserId)
                .IsRequired();
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(e => e.CreatedByUser)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
