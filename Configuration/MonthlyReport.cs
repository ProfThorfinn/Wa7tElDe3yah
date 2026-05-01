using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wa7at_ElDr3yah_API.Models;

namespace Wa7at_ElDr3yah_API.Configurations
{
    public class MonthlyReportConfiguration : IEntityTypeConfiguration<MonthlyReport>
    {
        public void Configure(EntityTypeBuilder<MonthlyReport> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Month)
                .IsRequired();

            builder.Property(r => r.Year)
                .IsRequired();

            builder.Property(r => r.TotalRevenue)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(r => r.TotalExpenses)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(r => r.NetProfitOrLoss)
                .HasPrecision(10, 2);

            builder.Property(r => r.ProfitLossPercentage)
                .HasPrecision(5, 2);

            builder.Property(r => r.PdfFilePath)
                .HasMaxLength(500);

            builder.Property(r => r.GeneratedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(r => r.GeneratedByUser)
                .WithMany(u => u.MonthlyReports)
                .HasForeignKey(r => r.GeneratedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => new { r.Month, r.Year });
        }
    }
}