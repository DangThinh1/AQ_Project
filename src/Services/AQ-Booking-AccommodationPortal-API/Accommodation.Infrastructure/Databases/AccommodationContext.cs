using Accommodation.Infrastructure.Databases.Entities;
using Microsoft.EntityFrameworkCore;

namespace Accommodation.Infrastructure.Databases
{
    public partial class AccommodationContext : DbContext
    {
        public AccommodationContext()
        {
        }

        public AccommodationContext(DbContextOptions<AccommodationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HotelAttributeValues> HotelAttributeValues { get; set; }
        public virtual DbSet<HotelAttributes> HotelAttributes { get; set; }
        public virtual DbSet<HotelCounters> HotelCounters { get; set; }
        public virtual DbSet<HotelFileStreams> HotelFileStreams { get; set; }
        public virtual DbSet<HotelInformationDetails> HotelInformationDetails { get; set; }
        public virtual DbSet<HotelInformations> HotelInformations { get; set; }
        public virtual DbSet<HotelInventories> HotelInventories { get; set; }
        public virtual DbSet<HotelInventoryAttributeValues> HotelInventoryAttributeValues { get; set; }
        public virtual DbSet<HotelInventoryAttributes> HotelInventoryAttributes { get; set; }
        public virtual DbSet<HotelInventoryDescriptions> HotelInventoryDescriptions { get; set; }
        public virtual DbSet<HotelInventoryFileStreams> HotelInventoryFileStreams { get; set; }
        public virtual DbSet<HotelInventoryRates> HotelInventoryRates { get; set; }
        public virtual DbSet<HotelMerchantUsers> HotelMerchantUsers { get; set; }
        public virtual DbSet<HotelMerchants> HotelMerchants { get; set; }
        public virtual DbSet<HotelReservationDetails> HotelReservationDetails { get; set; }
        public virtual DbSet<HotelReservationProcessingFees> HotelReservationProcessingFees { get; set; }
        public virtual DbSet<HotelReservations> HotelReservations { get; set; }
        public virtual DbSet<Hotels> Hotels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<HotelAttributeValues>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttributeCategoryFid).HasColumnName("AttributeCategoryFID");

                entity.Property(e => e.AttributeFid).HasColumnName("AttributeFID");

                entity.Property(e => e.AttributeValue).HasMaxLength(256);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.HotelFid).HasColumnName("HotelFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<HotelAttributes>(entity =>
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

            modelBuilder.Entity<HotelCounters>(entity =>
            {
                entity.HasKey(e => e.HotelId)
                    .HasName("PK__HotelCou__46023BBFFFCBC0E2");

                entity.Property(e => e.HotelId)
                    .HasColumnName("HotelID")
                    .ValueGeneratedNever();

                entity.Property(e => e.HotelUniqueId)
                    .IsRequired()
                    .HasColumnName("HotelUniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<HotelFileStreams>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileCategoryFid).HasColumnName("FileCategoryFID");

                entity.Property(e => e.FileCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.FileTypeResKey).HasMaxLength(150);

                entity.Property(e => e.HotelFid).HasColumnName("HotelFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<HotelInformationDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.InformationFid).HasColumnName("InformationFID");

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ShortDescriptions).HasMaxLength(1000);

                entity.Property(e => e.Title).HasMaxLength(456);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<HotelInformations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.DefaultTitle).HasMaxLength(456);

                entity.Property(e => e.HotelFid).HasColumnName("HotelFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<HotelInventories>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.BedQuantity).HasDefaultValueSql("((1))");

                entity.Property(e => e.HotelFid).HasColumnName("HotelFID");

                entity.Property(e => e.HotelRoomCode).HasMaxLength(50);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MaxOccupiedPerson).HasDefaultValueSql("((1))");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.Property(e => e.RoomName).HasMaxLength(456);

                entity.Property(e => e.RoomTypeFid).HasColumnName("RoomTypeFID");

                entity.Property(e => e.RoomTypeResKey).HasMaxLength(150);
            });

            modelBuilder.Entity<HotelInventoryAttributeValues>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttributeCategoryFid).HasColumnName("AttributeCategoryFID");

                entity.Property(e => e.AttributeFid).HasColumnName("AttributeFID");

                entity.Property(e => e.AttributeValue).HasMaxLength(256);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.InventoryFid).HasColumnName("InventoryFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<HotelInventoryAttributes>(entity =>
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

            modelBuilder.Entity<HotelInventoryDescriptions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.InventoryFid).HasColumnName("InventoryFID");

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ShortDescriptions).HasMaxLength(1000);

                entity.Property(e => e.Title).HasMaxLength(456);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<HotelInventoryFileStreams>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileCategoryFid).HasColumnName("FileCategoryFID");

                entity.Property(e => e.FileCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.FileTypeResKey).HasMaxLength(150);

                entity.Property(e => e.InventoryFid).HasColumnName("InventoryFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<HotelInventoryRates>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.InventoryFid).HasColumnName("InventoryFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);
            });

            modelBuilder.Entity<HotelMerchantUsers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.MerchantName).HasMaxLength(100);

                entity.Property(e => e.MerchantUserRoleFid).HasColumnName("MerchantUserRoleFID");

                entity.Property(e => e.MerchantUserRoleResKey).HasMaxLength(150);

                entity.Property(e => e.UserEmail).HasMaxLength(100);

                entity.Property(e => e.UserFid).HasColumnName("UserFID");

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<HotelMerchants>(entity =>
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

            modelBuilder.Entity<HotelReservationDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.HotelFid).HasColumnName("HotelFID");

                entity.Property(e => e.InventoryFid).HasColumnName("InventoryFID");

                entity.Property(e => e.InvetoryRateFid).HasColumnName("InvetoryRateFID");

                entity.Property(e => e.ItemName).HasMaxLength(456);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.ReservationsFid).HasColumnName("ReservationsFID");
            });

            modelBuilder.Entity<HotelReservationProcessingFees>(entity =>
            {
                entity.HasKey(e => e.ReservationsFid)
                    .HasName("PK__HotelRes__ABD9742E5E3D2381");

                entity.Property(e => e.ReservationsFid)
                    .HasColumnName("ReservationsFID")
                    .ValueGeneratedNever();

                entity.Property(e => e.GeneratedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);
            });

            modelBuilder.Entity<HotelReservations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Adult).HasDefaultValueSql("((1))");

                entity.Property(e => e.CancelReason).HasMaxLength(2000);

                entity.Property(e => e.ContactNo).HasMaxLength(20);

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.CustomerFid).HasColumnName("CustomerFID");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CustomerNote).HasMaxLength(2000);

                entity.Property(e => e.DiningDate).HasColumnType("datetime");

                entity.Property(e => e.HotelFid).HasColumnName("HotelFID");

                entity.Property(e => e.PaymentCurrency).HasMaxLength(10);

                entity.Property(e => e.PaymentExchangeDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessedDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessedRemark).HasMaxLength(2000);

                entity.Property(e => e.ReservationDate).HasColumnType("datetime");

                entity.Property(e => e.ReservationEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SpecialRequestDescriptions).HasMaxLength(2000);

                entity.Property(e => e.StatusFid)
                    .HasColumnName("StatusFID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StatusResKey).HasMaxLength(150);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<Hotels>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address1).HasMaxLength(500);

                entity.Property(e => e.Address2).HasMaxLength(500);

                entity.Property(e => e.BrandCode).HasMaxLength(50);

                entity.Property(e => e.BrandName).HasMaxLength(456);

                entity.Property(e => e.ChainCode).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(500);

                entity.Property(e => e.CityFid).HasColumnName("CityFID");

                entity.Property(e => e.Country).HasMaxLength(500);

                entity.Property(e => e.CountryFid).HasColumnName("CountryFID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.District).HasMaxLength(500);

                entity.Property(e => e.FullAddress).HasMaxLength(2000);

                entity.Property(e => e.HotelCategoryFid).HasColumnName("HotelCategoryFID");

                entity.Property(e => e.HotelCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.HotelCode).HasMaxLength(50);

                entity.Property(e => e.HotelName)
                    .IsRequired()
                    .HasMaxLength(456);

                entity.Property(e => e.HotelShortName).HasMaxLength(256);

                entity.Property(e => e.HotelTypeFid).HasColumnName("HotelTypeFID");

                entity.Property(e => e.HotelTypeResKey).HasMaxLength(150);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Latitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.MapCapturedFileStreamFid).HasColumnName("MapCapturedFileStreamFID");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.SourceFid)
                    .HasColumnName("SourceFID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SourceResKey).HasMaxLength(150);

                entity.Property(e => e.StartOperation).HasMaxLength(50);

                entity.Property(e => e.StatusFid).HasColumnName("StatusFID");

                entity.Property(e => e.StatusResKey).HasMaxLength(150);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.WhenBuild).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}