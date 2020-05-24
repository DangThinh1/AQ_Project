using AQDiningPortal.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Infrastructure.Database
{
    public class DiningSearchContext : DbContext
    {
        public virtual DbSet<Restaurants> Restaurants { get; set; }
        public virtual DbSet<RestaurantAttributeValues> RestaurantAttributeValues { get; set; }
        public virtual DbSet<RestaurantInformations> RestaurantInformations { get; set; }
        public virtual DbSet<RestaurantInformationDetails> RestaurantInformationDetails { get; set; }
        public virtual DbSet<RestaurantOtherInformations> RestaurantOtherInformations { get; set; }
        public virtual DbSet<RestaurantBusinessDays> RestaurantBusinessDays { get; set; }
        public virtual DbSet<RestaurantNonBusinessDays> RestaurantNonBusinessDays { get; set; }
        public virtual DbSet<RestaurantReservationFees> RestaurantReservationFees { get; set; }
        public virtual DbSet<RestaurantFileStreams> RestaurantFileStreams { get; set; }
        public virtual DbSet<RestaurantMenus> RestaurantMenus { get; set; }
        public virtual DbSet<RestaurantCounters> RestaurantCounters { get; set; }
        public virtual DbSet<RestaurantMerchants> RestaurantMerchants { get; set; }
        public virtual DbSet<RestaurantAttributes> RestaurantAttributes { get; set; }
        public virtual DbSet<RestaurantBusinessDayOperations> RestaurantBusinessDayOperations { get; set; }
        public virtual DbSet<RestaurantMenuCounters> RestaurantMenuCounters { get; set; }
        public virtual DbSet<RestaurantMenuInfoDetails> RestaurantMenuInfoDetails { get; set; }
        public virtual DbSet<RestaurantMenuPricings> RestaurantMenuPricings { get; set; }
        public virtual DbSet<RestaurantMerchantAgreements> RestaurantMerchantAgreements { get; set; }
        public virtual DbSet<RestaurantMerchantAqmgts> RestaurantMerchantAqmgts { get; set; }
        public virtual DbSet<RestaurantMerchantUsers> RestaurantMerchantUsers { get; set; }
        public virtual DbSet<RestaurantVenues> RestaurantVenues { get; set; }
        public virtual DbSet<RestaurantVenuePricings> RestaurantVenuePricings { get; set; }
        public virtual DbSet<RestaurantVenueInfoDetails> RestaurantVenueInfoDetails { get; set; }
        public DiningSearchContext()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        public DiningSearchContext(DbContextOptions<DiningSearchContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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

            #region SQL Scalar Function
            modelBuilder.HasDbFunction(this.GetType().GetMethod("GetCuisines"), options =>
            {
                options.HasName("fnCuisinesVal");
                options.HasSchema("dbo");
            });
            modelBuilder.HasDbFunction(this.GetType().GetMethod("GetfnIsBusinessDayOperation"), options =>
            {
                options.HasName("fnIsBusinessDayOperationVal");
                options.HasSchema("dbo");
            });
            modelBuilder.HasDbFunction(this.GetType().GetMethod("GetfnRestaurantMenuID"), options =>
            {
                options.HasName("fnRestaurantMenuIDVal");
                options.HasSchema("dbo");
            });
            modelBuilder.HasDbFunction(this.GetType().GetMethod("GetfnRestaurantImageIDVal"), options =>
            {
                options.HasName("fnRestaurantImageIDVal");
                options.HasSchema("dbo");
            });
            modelBuilder.HasDbFunction(this.GetType().GetMethod("GetfnResMerchantImageIDVal"), options =>
            {
                options.HasName("fnResMerchantImageIDVal");
                options.HasSchema("dbo");
            });
            #endregion
            #region restaurant relatioship
            modelBuilder.Entity<RestaurantAttributeValues>()
                .HasOne(p => p.Restaurant).WithMany(r => r.AttributeValues).HasForeignKey(p => p.RestaurantFid);

            modelBuilder.Entity<RestaurantInformations>()
                .HasOne(p => p.Restaurant).WithMany(r => r.Informations).HasForeignKey(p => p.RestaurantFid);

            modelBuilder.Entity<RestaurantInformationDetails>()
                .HasOne(p => p.Information).WithMany(r => r.InformationDetails).HasForeignKey(p => p.InformationFid);

            modelBuilder.Entity<RestaurantOtherInformations>()
                .HasOne(p => p.Restaurant).WithMany(r => r.OtherInformations).HasForeignKey(p => p.RestaurantFid);

            modelBuilder.Entity<RestaurantBusinessDays>()
                .HasOne(p => p.Restaurant).WithMany(r => r.BusinessDays).HasForeignKey(p => p.RestaurantFid);

            modelBuilder.Entity<RestaurantBusinessDayOperations>()
                .HasOne(p => p.RestaurantBusinessDay).WithMany(r => r.BusinessDayOperations).HasForeignKey(p => p.RestaurantBusinessDayFid);

            modelBuilder.Entity<RestaurantNonBusinessDays>()
                .HasOne(p => p.Restaurant).WithMany(r => r.NonBusinessDays).HasForeignKey(p => p.RestaurantFid);

            modelBuilder.Entity<RestaurantReservationFees>()
                .HasOne(p => p.Restaurant).WithMany(r => r.ReservationFees).HasForeignKey(p => p.RestaurantFid);

            modelBuilder.Entity<RestaurantFileStreams>()
                .HasOne(p => p.Restaurant).WithMany(r => r.FileStreams).HasForeignKey(p => p.RestaurantFid);

            modelBuilder.Entity<RestaurantMenuInfoDetails>()
                .HasOne(p => p.Menu).WithMany(r => r.InfoDetails).HasForeignKey(p => p.MenuFid);

            modelBuilder.Entity<RestaurantMenuPricings>()
                .HasOne(p => p.Menu).WithMany(r => r.Pricings).HasForeignKey(p => p.MenuFid);

            modelBuilder.Entity<RestaurantMenus>()
                .HasOne(p => p.Restaurant).WithMany(r => r.Menus).HasForeignKey(p => p.RestaurantFid);

            modelBuilder.Entity<Restaurants>()
                .HasOne(p => p.Counter).WithOne(r => r.Restaurant).HasForeignKey<RestaurantCounters>(p => p.RestaurantId);
            #endregion

            modelBuilder.Entity<Restaurants>()
                .HasOne(p => p.Merchant).WithMany(r => r.Restaurants).HasForeignKey(p => p.MerchantFid);
            modelBuilder.Query<Core.Models.Restaurants.RestaurantCuisineDetailViewModel>();
        }

        [DbFunction("fnRestaurantImageIDVal", Schema = "dbo")]
        public int? GetfnRestaurantImageIDVal(int RestaurantId, int FileType)
        { throw new NotImplementedException(); }
        [DbFunction("fnRestaurantMenuIDVal", Schema = "dbo")]
        public int? GetfnRestaurantMenuID(int RestaurantId)
        { throw new NotImplementedException(); }

        [DbFunction("fnCuisinesVal", Schema = "dbo")]
        public string GetCuisines(int RestaurantId, int AttributeCategoryFID)
        { throw new NotImplementedException(); }

        [DbFunction("fnIsBusinessDayOperationVal", Schema = "dbo")]
        public bool GetfnIsBusinessDayOperation(int RestaurantId, string SearhDate)
        { throw new NotImplementedException(); }
        [DbFunction("fnResMerchantImageIDVal", Schema = "dbo")]
        public int GetfnResMerchantImageIDVal(int MerchantFId, int FileType)
        { throw new NotImplementedException(); }

    }
}
