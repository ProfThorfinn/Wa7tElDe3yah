using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wa7at_ElDr3yah_API.Models;

namespace Wa7at_ElDr3yah_API.Configurations
{
    public class CapitalConfiguration : IEntityTypeConfiguration<Capital>
    {
        public void Configure(EntityTypeBuilder<Capital> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Amount)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(c => c.StartDate)
                .IsRequired();

            builder.Property(c => c.Notes)
                .HasMaxLength(500);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(c => c.CreatedByUser)
                .WithMany(u => u.Capitals)
                .HasForeignKey(c => c.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}