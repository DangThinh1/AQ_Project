using Microsoft.EntityFrameworkCore;
using AccommodationMerchant.Infrastructure.Databases.Entities;

namespace AccommodationMerchant.Infrastructure.Databases
{
    public partial class AccommodationContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotels>(entity => {
                entity.HasMany(k => k.Inventories)
                      .WithOne(k => k.Hotel)
                      .HasForeignKey(k=> k.HotelFid);
            });
        }
    }
}