using AQDiningPortal.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQDiningPortal.Infrastructure.Database
{
    public class DiningReservationContext : DbContext
    {
        public DiningReservationContext()
        {
        }

        public DiningReservationContext(DbContextOptions<DiningReservationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public virtual DbSet<RestaurantDiningReservationDetails> RestaurantDiningReservationDetails { get; set; }
        public virtual DbSet<RestaurantDiningReservationPaymentLogs> RestaurantDiningReservationPaymentLogs { get; set; }
        public virtual DbSet<RestaurantDiningReservations> RestaurantDiningReservations { get; set; }
    }
}
