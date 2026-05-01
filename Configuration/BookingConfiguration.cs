using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wa7at_ElDr3yah_API.Models;

namespace Wa7at_ElDr3yah_API.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.TotalPrice)
                .HasPrecision(10, 2);

            builder.Property(b => b.PaidAmount)
                .HasPrecision(10, 2);

            builder.Property(b => b.RemainingAmount)
                .HasPrecision(10, 2);

            builder.HasOne(b => b.CreatedByUser)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(b => b.BookingDate)
                .IsUnique();
        }
    }
}