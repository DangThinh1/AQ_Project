using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AQBooking.Admin.Infrastructure.Databases.YachtEntities
{
    public partial class AQYachtContext : DbContext
    {
        public AQYachtContext()
        {
        }

        public AQYachtContext(DbContextOptions<AQYachtContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PortLocations> PortLocations { get; set; }
        public virtual DbSet<YachtMerchantAqmgts> YachtMerchantAqmgts { get; set; }
        public virtual DbSet<YachtMerchantCharterFeeOptions> YachtMerchantCharterFeeOptions { get; set; }
        public virtual DbSet<YachtMerchantFileStreams> YachtMerchantFileStreams { get; set; }
        public virtual DbSet<YachtMerchantUsers> YachtMerchantUsers { get; set; }
        public virtual DbSet<YachtMerchants> YachtMerchants { get; set; }
        public virtual DbSet<YachtAttributeValues> YachtAttributeValues { get; set; }
        public virtual DbSet<YachtAttributes> YachtAttributes { get; set; }
        public virtual DbSet<YachtTourAttributeValues> YachtTourAttributeValues { get; set; }
        public virtual DbSet<YachtTourAttributes> YachtTourAttributes { get; set; }
        public virtual DbSet<YachtTourCategories> YachtTourCategories { get; set; }
        public virtual DbSet<YachtTourCategoryInfomations> YachtTourCategoryInfomations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");
         
            modelBuilder.Entity<YachtTourCategories>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DefaultName).HasMaxLength(256);

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.Property(e => e.ResourceKey).HasMaxLength(150);
            });

            modelBuilder.Entity<YachtTourCategoryInfomations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ShortDescription).HasMaxLength(2000);

                entity.Property(e => e.TourCategoryFid).HasColumnName("TourCategoryFID");

                entity.Property(e => e.TourCategoryResourceKey).HasMaxLength(150);
            });


            modelBuilder.Entity<PortLocations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City).HasMaxLength(256);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PickupPointName).HasMaxLength(256);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.ZoneDistrict).HasMaxLength(256);
            });

            modelBuilder.Entity<YachtMerchantAqmgts>(entity =>
            {
                entity.ToTable("YachtMerchantAQMgts");

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

            modelBuilder.Entity<YachtMerchantCharterFeeOptions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookingFeeOptionFid).HasColumnName("BookingFeeOptionFID");

                entity.Property(e => e.BookingFeeOptionResKey).HasMaxLength(150);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.Property(e => e.SchemeBasedOptionFid).HasColumnName("SchemeBasedOptionFID");

                entity.Property(e => e.SchemeBasedOptionResKey).HasMaxLength(150);
            });

            modelBuilder.Entity<YachtMerchantFileStreams>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");
            });

            modelBuilder.Entity<YachtMerchantUsers>(entity =>
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

            modelBuilder.Entity<YachtMerchants>(entity =>
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

                entity.Property(e => e.LandingPageOptionFid).HasColumnName("LandingPageOptionFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantName).HasMaxLength(456);

                entity.Property(e => e.MerchantTypeFid)
                    .HasColumnName("MerchantTypeFID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MerchantTypeResKey).HasMaxLength(150);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.State).HasMaxLength(456);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.ZipCode).HasMaxLength(10);

                entity.Property(e => e.ZoneFid).HasColumnName("ZoneFID");
            });

            modelBuilder.Entity<YachtAttributeValues>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttributeCategoryFid).HasColumnName("AttributeCategoryFID");

                entity.Property(e => e.AttributeFid).HasColumnName("AttributeFID");

                entity.Property(e => e.AttributeValue).HasMaxLength(256);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtAttributes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttributeCategoryFid).HasColumnName("AttributeCategoryFID");

                entity.Property(e => e.AttributeName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IconCssClass).HasMaxLength(50);

                entity.Property(e => e.IsDefault).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(250);

                entity.Property(e => e.ResourceKey)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<YachtTourAttributeValues>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttributeCategoryFid).HasColumnName("AttributeCategoryFID");

                entity.Property(e => e.AttributeFid).HasColumnName("AttributeFID");

                entity.Property(e => e.AttributeValue).HasMaxLength(256);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.YachtTourFid).HasColumnName("YachtTourFID");
            });

            modelBuilder.Entity<YachtTourAttributes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttributeCategoryFid).HasColumnName("AttributeCategoryFID");

                entity.Property(e => e.AttributeName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IconCssClass).HasMaxLength(50);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(250);

                entity.Property(e => e.ResourceKey)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });
        }
    }
}
