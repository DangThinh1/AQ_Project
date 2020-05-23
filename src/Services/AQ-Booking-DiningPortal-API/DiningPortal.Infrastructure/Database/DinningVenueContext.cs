using AQDiningPortal.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Infrastructure.Database
{
    public partial class DinningVenueContext : DbContext
    {
        public DinningVenueContext()
        {
        }

        public DinningVenueContext(DbContextOptions<DinningVenueContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RestaurantVenueInfoDetails> RestaurantVenueInfoDetails { get; set; }
        public virtual DbSet<RestaurantVenuePricings> RestaurantVenuePricings { get; set; }
        public virtual DbSet<RestaurantVenues> RestaurantVenues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=172.16.10.8,1400;Initial Catalog=AQDining;User=sa;Password=tester@2019;Integrated Security=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<RestaurantVenueInfoDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.VenueFid).HasColumnName("VenueFID");
            });

            modelBuilder.Entity<RestaurantVenuePricings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.HasPrice)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.VenueFid).HasColumnName("VenueFID");

                entity.Property(e => e.VenuePricingTypeFid).HasColumnName("VenuePricingTypeFID");

                entity.Property(e => e.VenuePricingTypeResKey).HasMaxLength(150);
            });

            modelBuilder.Entity<RestaurantVenues>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DefaultName).HasMaxLength(256);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RestaurantFid).HasColumnName("RestaurantFID");

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);
            });
        }
    }
}
