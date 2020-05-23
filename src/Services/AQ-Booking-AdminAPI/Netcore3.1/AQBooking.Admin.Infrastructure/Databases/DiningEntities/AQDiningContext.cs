using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AQBooking.Admin.Infrastructure.Databases.DiningEntities
{
    public partial class AQDiningContext : DbContext
    {
        public AQDiningContext()
        {
        }

        public AQDiningContext(DbContextOptions<AQDiningContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RestaurantMerchantAqmgts> RestaurantMerchantAqmgts { get; set; }
        public virtual DbSet<RestaurantMerchantUsers> RestaurantMerchantUsers { get; set; }
        public virtual DbSet<RestaurantMerchants> RestaurantMerchants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=172.16.10.137,1600;Initial Catalog=AQDining;User=erp;Password=erp2017;Integrated Security=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<RestaurantMerchantAqmgts>(entity =>
            {
                entity.ToTable("RestaurantMerchantAQMgts");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AqadminUserFid).HasColumnName("AQAdminUserFID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.MerchantName).HasMaxLength(100);

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.Property(e => e.UserEmail).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<RestaurantMerchantUsers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.MerchantName).HasMaxLength(100);

                entity.Property(e => e.UserEmail).HasMaxLength(100);

                entity.Property(e => e.UserFid).HasColumnName("UserFID");

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<RestaurantMerchants>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountSize).HasDefaultValueSql("((5))");

                entity.Property(e => e.Address1).HasMaxLength(456);

                entity.Property(e => e.Address2).HasMaxLength(456);

                entity.Property(e => e.City).HasMaxLength(256);

                entity.Property(e => e.ContactNumber1).HasMaxLength(50);

                entity.Property(e => e.ContactNumber2).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(256);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress1).HasMaxLength(100);

                entity.Property(e => e.EmailAddress2).HasMaxLength(100);

                entity.Property(e => e.ExpiredDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantName).HasMaxLength(456);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.State).HasMaxLength(456);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.ZipCode).HasMaxLength(10);

                entity.Property(e => e.ZoneFid).HasColumnName("ZoneFID");
            });
        }
    }
}
